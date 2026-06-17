USE SACRSoft;

DROP PROCEDURE IF EXISTS sp_ProcesarYDispersarWebhookBokun;
DELIMITER //

CREATE PROCEDURE sp_ProcesarYDispersarWebhookBokun (
    IN p_raw_json LONGTEXT
)
BEGIN
    -- Declaración de variables para el Cliente
    DECLARE v_cli_nombres VARCHAR(50);
    DECLARE v_cli_apellidos VARCHAR(50);
    DECLARE v_cli_correo VARCHAR(80);
    DECLARE v_id_cliente INT DEFAULT NULL;

    -- Declaración de variables para el Usuario (extranetUser)
    DECLARE v_usr_nombres VARCHAR(50);
    DECLARE v_usr_apellidos VARCHAR(50);
    DECLARE v_usr_correo VARCHAR(35);
    DECLARE v_id_usuario INT DEFAULT NULL;

    -- Declaración de variables para la Reserva
    DECLARE v_res_bookingId VARCHAR(80);
    DECLARE v_res_fecha_reg DATETIME;
    DECLARE v_res_estado VARCHAR(15);
    DECLARE v_res_cant_boletos INT;
    DECLARE v_res_monto_total DECIMAL(12, 2);
    DECLARE v_res_fecha_modif DATETIME;
    DECLARE v_res_canal VARCHAR(50);
    DECLARE v_res_impuestos DECIMAL(12, 2);
    DECLARE v_res_codigo_bokun VARCHAR(80);
    DECLARE v_id_reserva INT;

    -- Variables de control para iterar el Arreglo de Actividades
    DECLARE v_array_len INT;
    DECLARE i INT DEFAULT 0;
    
    -- Variables para el Servicio y Detalle
    DECLARE v_srv_nombre VARCHAR(50);
    DECLARE v_srv_descripcion VARCHAR(80);
    DECLARE v_srv_precio DECIMAL(10, 2);
    DECLARE v_srv_duracion DECIMAL(5, 2);
    DECLARE v_srv_idioma VARCHAR(35);
    DECLARE v_srv_capacidad INT;
    DECLARE v_srv_recojo CHAR(1);
    DECLARE v_srv_destino VARCHAR(40);
    DECLARE v_id_servicio INT;
    
    DECLARE v_det_cantidad INT;
    DECLARE v_det_subtotal DECIMAL(12, 2);
    DECLARE v_id_detalle INT;
    DECLARE v_id_log_webhook INT;

    -- =================================================================
    -- 1. EXTRACCIÓN Y DISPERSIÓN DE DATOS DEL CLIENTE (customer)
    -- =================================================================
    SET v_cli_correo = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.email'));
    SET v_cli_nombres = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.firstName'));
    SET v_cli_apellidos = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.lastName'));

    -- Validaciones básicas por si los campos vienen nulos en la petición
    IF v_cli_nombres IS NULL OR v_cli_nombres = 'null' THEN SET v_cli_nombres = 'CLIENTE'; END IF;
    IF v_cli_apellidos IS NULL OR v_cli_apellidos = 'null' THEN SET v_cli_apellidos = 'BOKUN'; END IF;

    -- Verificar si el cliente ya existe en la base de datos para no duplicarlo
    SELECT id_cliente INTO v_id_cliente FROM Cliente WHERE correo = v_cli_correo LIMIT 1;
    
    -- Si no existe, lo registramos llamando a tu procedimiento CRUD
    IF v_id_cliente IS NULL THEN
        CALL sp_InsertCliente(
            SUBSTRING(v_cli_nombres, 1, 50), 
            SUBSTRING(v_cli_apellidos, 1, 50), 
            'OTROS',                       -- tipo_documento por defecto
            '00000000',                    -- numero_documento por defecto
            SUBSTRING(v_cli_correo, 1, 80), 
            'PE',                          -- nacionalidad
            CURDATE(),                     -- fecha_registro
            '00000000',                    -- numero_contacto
            '2000-01-01',                  -- fecha_nacimiento
            v_id_cliente                   -- OUT: retorna el id generado
        );
    END IF;

    -- =================================================================
    -- 2. EXTRACCIÓN Y DISPERSIÓN DEL USUARIO OPERADOR (extranetUser)
    -- =================================================================
    SET v_usr_correo = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.username'));
    SET v_usr_nombres = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.firstName'));
    SET v_usr_apellidos = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.lastName'));

    IF v_usr_correo IS NOT NULL AND v_usr_correo <> 'null' THEN
        -- Verificar si el usuario gestor ya existe
        SELECT id_usuario INTO v_id_usuario FROM Usuario WHERE correo = SUBSTRING(v_usr_correo, 1, 35) LIMIT 1;
        
        -- Si no existe, se inserta
        IF v_id_usuario IS NULL THEN
            CALL sp_InsertUsuario(
                SUBSTRING(v_usr_nombres, 1, 50), 
                SUBSTRING(v_usr_apellidos, 1, 50), 
                'DNI', 
                '00000000', 
                SUBSTRING(v_usr_correo, 1, 35), 
                SHA2('PasswordBokun2026!', 256), -- Contraseña encriptada temporal
                '00000000', 
                'EXTRANET_USER',                 -- tipo_usuario
                v_id_usuario                     -- OUT: retorna el id generado
            );
        END IF;
    ELSE
        -- Si no hay un usuario registrado en el backend que operó la acción, se asigna NULL
        SET v_id_usuario = NULL; 
    END IF;

    -- =================================================================
    -- 3. EXTRACCIÓN Y DISPERSIÓN DE LA RESERVA (Master)
    -- =================================================================
    SET v_res_bookingId = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.bookingId'));
    -- Conversión del Timestamp de milisegundos de creación a DATETIME
    SET v_res_fecha_reg = FROM_UNIXTIME(JSON_EXTRACT(p_raw_json, '$.creationDate') / 1000);
    SET v_res_estado = SUBSTRING(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.status')), 1, 15);
    SET v_res_monto_total = JSON_EXTRACT(p_raw_json, '$.totalPrice');
    SET v_res_canal = SUBSTRING(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.bookingChannel.title')), 1, 50);
    SET v_res_impuestos = JSON_EXTRACT(p_raw_json, '$.invoice.totalTaxAsMoney.amount');
    SET v_res_codigo_bokun = SUBSTRING(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.confirmationCode')), 1, 80);
    
    -- Conversión del timestamp de última modificación o cancelación
    IF JSON_EXTRACT(p_raw_json, '$.cancellationDate') IS NOT NULL AND JSON_EXTRACT(p_raw_json, '$.cancellationDate') <> 'null' THEN
        SET v_res_fecha_modif = FROM_UNIXTIME(JSON_EXTRACT(p_raw_json, '$.cancellationDate') / 1000);
    ELSE
        SET v_res_fecha_modif = NOW();
    END IF;

    -- Extraemos la cantidad de boletos basándonos en el primer ítem del listado
    SET v_res_cant_boletos = JSON_EXTRACT(p_raw_json, '$.activityBookings[0].totalParticipants');
    IF v_res_cant_boletos IS NULL THEN SET v_res_cant_boletos = 1; END IF;

    -- Inserción en la tabla Reserva a través de su CRUD correspondiente
    CALL sp_InsertReserva(
        v_res_fecha_reg, 
        v_res_estado, 
        v_res_cant_boletos, 
        v_res_monto_total, 
        v_res_fecha_modif, 
        v_res_canal, 
        v_res_impuestos, 
        v_res_codigo_bokun, 
        v_id_usuario,       -- FK mapeada
        v_id_cliente,       -- FK mapeada
        v_id_reserva        -- OUT: ID de la reserva creada para usar en el detalle
    );

    -- =================================================================
    -- 4. ITERACIÓN Y DISPERSIÓN DE LOS DETALLES (activityBookings Array)
    -- =================================================================
    SET v_array_len = JSON_LENGTH(JSON_EXTRACT(p_raw_json, '$.activityBookings'));
    
    WHILE i < v_array_len DO
        -- Extracción de la data específica de la Actividad en el índice actual
        SET v_srv_nombre = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].title')));
        SET v_srv_descripcion = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].product.excerpt')));
        SET v_srv_precio = JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalPrice'));
        SET v_srv_duracion = JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.durationHours'));
        SET v_srv_idioma = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.languages[0]')));
        SET v_srv_capacidad = JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.passCapacity'));
        
        -- Mapeo de booleano a CHAR(1) para recogida ('S' o 'N')
        IF JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].pickup')) = true THEN
            SET v_srv_recojo = 'S';
        ELSE
            SET v_srv_recojo = 'N';
        END IF;
        
        -- Destino (obtenido a partir de la zona horaria asignada al proveedor o valor por defecto)
        SET v_srv_destino = JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.actualVendor.timeZone')));
        IF v_srv_destino IS NULL THEN SET v_srv_destino = 'America/Lima'; END IF;

        -- Controlar el tamaño de los VARCHAR permitidos según firmas de tus procedimientos
        SET v_srv_nombre = SUBSTRING(v_srv_nombre, 1, 50);
        SET v_srv_descripcion = SUBSTRING(v_srv_descripcion, 1, 80);
        SET v_srv_idioma = SUBSTRING(v_srv_idioma, 1, 35);
        SET v_srv_destino = SUBSTRING(v_srv_destino, 1, 40);

        -- Verificar si el servicio ya existe registrado en el catálogo de SACRSoft
        SELECT id_servicio INTO v_id_servicio FROM Servicio WHERE nombre = v_srv_nombre LIMIT 1;
        
        -- Si el servicio es nuevo se registra
        IF v_id_servicio IS NULL THEN
            CALL sp_InsertServicio(
                v_srv_nombre, 
                v_srv_descripcion, 
                v_srv_precio, 
                v_srv_duracion, 
                v_srv_idioma, 
                v_srv_capacidad, 
                v_srv_recojo, 
                v_srv_destino, 
                v_id_servicio   -- OUT: retorna el ID generado
            );
        END IF;

        -- Extracción de las cantidades específicas para el Detalle de la Reserva
        SET v_det_cantidad = JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalParticipants'));
        IF v_det_cantidad IS NULL THEN SET v_det_cantidad = 1; END IF;
        SET v_det_subtotal = JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalPrice'));

        -- Inserción del Detalle de la Reserva enlazado al Maestro
        CALL sp_InsertDetalle_Reserva(
            v_id_reserva, 
            v_id_servicio, 
            v_det_cantidad, 
            v_det_subtotal, 
            v_id_detalle        -- OUT: ID del detalle de reserva
        );

        -- Incrementar contador para pasar a la siguiente actividad del JSON
        SET i = i + 1;
    END WHILE;

    -- =================================================================
    -- 5. REGISTRO COMPLETO DEL HISTORIAL DEL WEBHOOK (WebhookLog)
    -- =================================================================
    CALL sp_InsertWebhookLog(
        v_res_bookingId, 
        NOW(), 
        p_raw_json,       -- Se almacena la cadena completa en formato LONGTEXT
        v_id_reserva, 
        v_id_log_webhook  -- OUT: ID del log generado
    );

END //
DELIMITER ;