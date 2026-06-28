-- ==================================================================
-- SCRIPT DE PROCEDIMIENTOS ALMACENADOS (CRUD)
-- Esquema: SACRSoft
-- Dialecto: MySQL
-- ==================================================================

USE SACRSoft;

-- ==========================================
-- CRUD para la tabla: Cliente
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertCliente;
DELIMITER //
CREATE PROCEDURE sp_InsertCliente (
    IN p_nombres VARCHAR(50),
    IN p_apellidos VARCHAR(50),
    IN p_tipo_documento VARCHAR(20),
    IN p_numero_documento VARCHAR(20),
    IN p_correo VARCHAR(80),
    IN p_nacionalidad VARCHAR(30),
    IN p_fecha_registro DATE,
    IN p_numero_contacto VARCHAR(20),
    IN p_fecha_nacimiento DATE,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Cliente (nombres, apellidos, tipo_documento, numero_documento, correo, nacionalidad, fecha_registro, numero_contacto, fecha_nacimiento)
    VALUES (p_nombres, p_apellidos, p_tipo_documento, p_numero_documento, p_correo, p_nacionalidad, p_fecha_registro, p_numero_contacto, p_fecha_nacimiento);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateCliente;
DELIMITER //
CREATE PROCEDURE sp_UpdateCliente (
    IN p_id_cliente INT,
    IN p_nombres VARCHAR(50),
    IN p_apellidos VARCHAR(50),
    IN p_tipo_documento VARCHAR(20),
    IN p_numero_documento VARCHAR(20),
    IN p_correo VARCHAR(80),
    IN p_nacionalidad VARCHAR(30),
    IN p_fecha_registro DATE,
    IN p_numero_contacto VARCHAR(20),
    IN p_fecha_nacimiento DATE
)
BEGIN
    UPDATE Cliente
    SET 
        nombres = p_nombres,
        apellidos = p_apellidos,
        tipo_documento = p_tipo_documento,
        numero_documento = p_numero_documento,
        correo = p_correo,
        nacionalidad = p_nacionalidad,
        fecha_registro = p_fecha_registro,
        numero_contacto = p_numero_contacto,
        fecha_nacimiento = p_fecha_nacimiento
    WHERE id_cliente = p_id_cliente;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteCliente;
DELIMITER //
CREATE PROCEDURE sp_DeleteCliente (
    IN p_id_cliente INT
)
BEGIN
    DELETE FROM Cliente
    WHERE id_cliente = p_id_cliente;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListClientes;
DELIMITER //
CREATE PROCEDURE spListClientes()
BEGIN
    SELECT * FROM Cliente;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListClienteById;
DELIMITER //
CREATE PROCEDURE sp_ListClienteById (
    IN p_id_cliente INT
)
BEGIN
    SELECT * FROM Cliente
    WHERE id_cliente = p_id_cliente;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Usuario
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertUsuario;
DELIMITER //
CREATE PROCEDURE sp_InsertUsuario (
    IN p_nombres VARCHAR(50),
    IN p_apellidos VARCHAR(50),
    IN p_tipo_documento VARCHAR(20),
    IN p_numero_documento VARCHAR(20),
    IN p_correo VARCHAR(35),
    IN p_contrasena VARCHAR(80),
    IN p_numero_contacto VARCHAR(20),
    IN p_tipo_usuario VARCHAR(50),
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Usuario (nombres, apellidos, tipo_documento, numero_documento, correo, contrasena, numero_contacto, tipo_usuario)
    VALUES (p_nombres, p_apellidos, p_tipo_documento, p_numero_documento, p_correo, p_contrasena, p_numero_contacto, p_tipo_usuario);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateUsuario;
DELIMITER //
CREATE PROCEDURE sp_UpdateUsuario (
    IN p_id_usuario INT,
    IN p_nombres VARCHAR(50),
    IN p_apellidos VARCHAR(50),
    IN p_tipo_documento VARCHAR(20),
    IN p_numero_documento VARCHAR(20),
    IN p_correo VARCHAR(35),
    IN p_contrasena VARCHAR(80),
    IN p_numero_contacto VARCHAR(20),
    IN p_tipo_usuario VARCHAR(50)
)
BEGIN
    UPDATE Usuario
    SET 
        nombres = p_nombres,
        apellidos = p_apellidos,
        tipo_documento = p_tipo_documento,
        numero_documento = p_numero_documento,
        correo = p_correo,
        contrasena = p_contrasena,
        numero_contacto = p_numero_contacto,
        tipo_usuario = p_tipo_usuario
    WHERE id_usuario = p_id_usuario;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteUsuario;
DELIMITER //
CREATE PROCEDURE sp_DeleteUsuario (
    IN p_id_usuario INT
)
BEGIN
    DELETE FROM Usuario
    WHERE id_usuario = p_id_usuario;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListUsuarios;
DELIMITER //
CREATE PROCEDURE spListUsuarios()
BEGIN
    SELECT * FROM Usuario;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListUsuarioById;
DELIMITER //
CREATE PROCEDURE sp_ListUsuarioById (
    IN p_id_usuario INT
)
BEGIN
    SELECT * FROM Usuario
    WHERE id_usuario = p_id_usuario;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Servicio
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertServicio;
DELIMITER //
CREATE PROCEDURE sp_InsertServicio (
    IN p_nombre VARCHAR(50),
    IN p_descripcion VARCHAR(80),
    IN p_precio_usd DECIMAL(10, 2),
    IN p_duracion_horas DECIMAL(5, 2),
    IN p_idioma_guia VARCHAR(35),
    IN p_capacidad_maxima INT,
    IN p_incluye_recojo CHAR(1),
    IN p_ciudad_destino VARCHAR(40),
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Servicio (nombre, descripcion, precio_usd, duracion_horas, idioma_guia, capacidad_maxima, incluye_recojo, ciudad_destino)
    VALUES (p_nombre, p_descripcion, p_precio_usd, p_duracion_horas, p_idioma_guia, p_capacidad_maxima, p_incluye_recojo, p_ciudad_destino);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateServicio;
DELIMITER //
CREATE PROCEDURE sp_UpdateServicio (
    IN p_id_servicio INT,
    IN p_nombre VARCHAR(50),
    IN p_descripcion VARCHAR(80),
    IN p_precio_usd DECIMAL(10, 2),
    IN p_duracion_horas DECIMAL(5, 2),
    IN p_idioma_guia VARCHAR(35),
    IN p_capacidad_maxima INT,
    IN p_incluye_recojo CHAR(1),
    IN p_ciudad_destino VARCHAR(40)
)
BEGIN
    UPDATE Servicio
    SET 
        nombre = p_nombre,
        descripcion = p_descripcion,
        precio_usd = p_precio_usd,
        duracion_horas = p_duracion_horas,
        idioma_guia = p_idioma_guia,
        capacidad_maxima = p_capacidad_maxima,
        incluye_recojo = p_incluye_recojo,
        ciudad_destino = p_ciudad_destino
    WHERE id_servicio = p_id_servicio;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteServicio;
DELIMITER //
CREATE PROCEDURE sp_DeleteServicio (
    IN p_id_servicio INT
)
BEGIN
    DELETE FROM Servicio
    WHERE id_servicio = p_id_servicio;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListServicios;
DELIMITER //
CREATE PROCEDURE spListServicios()
BEGIN
    SELECT * FROM Servicio;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListServicioById;
DELIMITER //
CREATE PROCEDURE sp_ListServicioById (
    IN p_id_servicio INT
)
BEGIN
    SELECT * FROM Servicio
    WHERE id_servicio = p_id_servicio;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Log_Auditoria
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertLogAuditoria;
DELIMITER //
CREATE PROCEDURE sp_InsertLogAuditoria (
    IN p_descripcion VARCHAR(150),
    IN p_accion VARCHAR(70),
    IN p_fecha_registro DATETIME,
    IN p_origenAccion VARCHAR(150),
    IN p_id_usuario INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Log_Auditoria (descripcion, accion, fecha_registro, origenAccion, id_usuario)
    VALUES (p_descripcion, p_accion, p_fecha_registro, p_origenAccion, p_id_usuario);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListLogAuditoria;
DELIMITER //
CREATE PROCEDURE spListLogAuditoria()
BEGIN
    SELECT * FROM Log_Auditoria;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListLogAuditoriaById;
DELIMITER //
CREATE PROCEDURE sp_ListLogAuditoriaById (
    IN p_idLogAuditoria INT
)
BEGIN
    SELECT * FROM Log_Auditoria
    WHERE idLogAuditoria = p_idLogAuditoria;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Notificacion
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertNotificacion;
DELIMITER //
CREATE PROCEDURE sp_InsertNotificacion (
    IN p_mensaje VARCHAR(150),
    IN p_tipo_notificacion VARCHAR(70),
    IN p_fecha_envio DATETIME,
    IN p_leido CHAR(1),
    IN p_id_usuario INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Notificacion (mensaje, tipo_notificacion, fecha_envio, leido, id_usuario)
    VALUES (p_mensaje, p_tipo_notificacion, p_fecha_envio, p_leido, p_id_usuario);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateNotificacion;
DELIMITER //
CREATE PROCEDURE sp_UpdateNotificacion (
    IN p_id_notificacion INT,
    IN p_mensaje VARCHAR(150),
    IN p_tipo_notificacion VARCHAR(70),
    IN p_fecha_envio DATETIME,
    IN p_leido CHAR(1),
    IN p_id_usuario INT
)
BEGIN
    UPDATE Notificacion
    SET 
        mensaje = p_mensaje,
        tipo_notificacion = p_tipo_notificacion,
        fecha_envio = p_fecha_envio,
        leido = p_leido,
        id_usuario = p_id_usuario
    WHERE id_notificacion = p_id_notificacion;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteNotificacion;
DELIMITER //
CREATE PROCEDURE sp_DeleteNotificacion (
    IN p_id_notificacion INT
)
BEGIN
    DELETE FROM Notificacion
    WHERE id_notificacion = p_id_notificacion;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListNotificaciones;
DELIMITER //
CREATE PROCEDURE spListNotificaciones()
BEGIN
    SELECT * FROM Notificacion;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListNotificacionById;
DELIMITER //
CREATE PROCEDURE sp_ListNotificacionById (
    IN p_id_notificacion INT
)
BEGIN
    SELECT * FROM Notificacion
    WHERE id_notificacion = p_id_notificacion;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Reserva
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertReserva;
DELIMITER //
CREATE PROCEDURE sp_InsertReserva (
    IN p_fecha_registro DATETIME,
    IN p_estado_reserva VARCHAR(15),
    IN p_cantidad_boletos INT,
    IN p_monto_total DECIMAL(12, 2),
    IN p_fecha_ultima_modif DATETIME,
    IN p_canal_venta VARCHAR(50),
    IN p_monto_impuestos DECIMAL(12, 2),
    IN p_codigo_bokun VARCHAR(80),
    IN p_id_usuario INT,
    IN p_id_cliente INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reserva (fecha_registro, estado_reserva, cantidad_boletos, monto_total, fecha_ultima_modif, canal_venta, monto_impuestos, codigo_bokun, id_usuario, id_cliente)
    VALUES (p_fecha_registro, p_estado_reserva, p_cantidad_boletos, p_monto_total, p_fecha_ultima_modif, p_canal_venta, p_monto_impuestos, p_codigo_bokun, p_id_usuario, p_id_cliente);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateReserva;
DELIMITER //
CREATE PROCEDURE sp_UpdateReserva (
    IN p_id_reserva INT,
    IN p_fecha_registro DATETIME,
    IN p_estado_reserva VARCHAR(15),
    IN p_cantidad_boletos INT,
    IN p_monto_total DECIMAL(12, 2),
    IN p_fecha_ultima_modif DATETIME,
    IN p_canal_venta VARCHAR(50),
    IN p_monto_impuestos DECIMAL(12, 2),
    IN p_codigo_bokun VARCHAR(80),
    IN p_id_usuario INT,
    IN p_id_cliente INT
)
BEGIN
    UPDATE Reserva
    SET 
        fecha_registro = p_fecha_registro,
        estado_reserva = p_estado_reserva,
        cantidad_boletos = p_cantidad_boletos,
        monto_total = p_monto_total,
        fecha_ultima_modif = p_fecha_ultima_modif,
        canal_venta = p_canal_venta,
        monto_impuestos = p_monto_impuestos,
        codigo_bokun = p_codigo_bokun,
        id_usuario = p_id_usuario,
        id_cliente = p_id_cliente
    WHERE id_reserva = p_id_reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteReserva;
DELIMITER //
CREATE PROCEDURE sp_DeleteReserva (
    IN p_id_reserva INT
)
BEGIN
    DELETE FROM Reserva
    WHERE id_reserva = p_id_reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListReservas;
DELIMITER //
CREATE PROCEDURE spListReservas()
BEGIN
    SELECT * FROM Reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListReservaById;
DELIMITER //
CREATE PROCEDURE sp_ListReservaById (
    IN p_id_reserva INT
)
BEGIN
    SELECT * FROM Reserva
    WHERE id_reserva = p_id_reserva;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Detalle_Reserva
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertDetalle_Reserva;
DELIMITER //
CREATE PROCEDURE sp_InsertDetalle_Reserva (
    IN p_id_reserva INT,
    IN p_id_servicio INT,
    IN p_cantidad INT,
    IN p_subtotal DECIMAL(12, 2),
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reserva (id_reserva, id_servicio, cantidad, subtotal)
    VALUES (p_id_reserva, p_id_servicio, p_cantidad, p_subtotal);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateDetalle_Reserva;
DELIMITER //
CREATE PROCEDURE sp_UpdateDetalle_Reserva (
    IN p_id_detalle_reserva INT,
    IN p_id_reserva INT,
    IN p_id_servicio INT,
    IN p_cantidad INT,
    IN p_subtotal DECIMAL(12, 2)
)
BEGIN
    UPDATE Detalle_Reserva
    SET 
        id_reserva = p_id_reserva,
        id_servicio = p_id_servicio,
        cantidad = p_cantidad,
        subtotal = p_subtotal
    WHERE id_detalle_reserva = p_id_detalle_reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteDetalle_Reserva;
DELIMITER //
CREATE PROCEDURE sp_DeleteDetalle_Reserva (
    IN p_id_detalle_reserva INT
)
BEGIN
    DELETE FROM Detalle_Reserva
    WHERE id_detalle_reserva = p_id_detalle_reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListDetalleReserva;
DELIMITER //
CREATE PROCEDURE spListDetalleReserva()
BEGIN
    SELECT * FROM Detalle_Reserva;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListDetalleReservaById;
DELIMITER //
CREATE PROCEDURE sp_ListDetalleReservaById (
    IN p_id_detalle_reserva INT
)
BEGIN
    SELECT * FROM Detalle_Reserva
    WHERE id_detalle_reserva = p_id_detalle_reserva;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: WebhookLog
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertWebhookLog;
DELIMITER //
CREATE PROCEDURE sp_InsertWebhookLog (
    IN p_bookingId VARCHAR(80),
    IN p_timestamp DATETIME,
    IN p_raw_data LONGTEXT,
    IN p_id_reserva INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO WebhookLog (bookingId, timestamp, raw_data, id_reserva)
    VALUES (p_bookingId, p_timestamp, p_raw_data, p_id_reserva);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateWebhookLog;
DELIMITER //
CREATE PROCEDURE sp_UpdateWebhookLog (
    IN p_id_log INT,
    IN p_bookingId VARCHAR(80),
    IN p_timestamp DATETIME,
    IN p_raw_data LONGTEXT,
    IN p_id_reserva INT
)
BEGIN
    UPDATE WebhookLog
    SET 
        bookingId = p_bookingId,
        timestamp = p_timestamp,
        raw_data = p_raw_data,
        id_reserva = p_id_reserva
    WHERE id_log = p_id_log;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteWebhookLog;
DELIMITER //
CREATE PROCEDURE sp_DeleteWebhookLog (
    IN p_id_log INT
)
BEGIN
    DELETE FROM WebhookLog
    WHERE id_log = p_id_log;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListWebhookLogs;
DELIMITER //
CREATE PROCEDURE spListWebhookLogs()
BEGIN
    SELECT * FROM WebhookLog;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListWebhookLogById;
DELIMITER //
CREATE PROCEDURE sp_ListWebhookLogById (
    IN p_id_log INT
)
BEGIN
    SELECT * FROM WebhookLog
    WHERE id_log = p_id_log;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Reclamo
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertReclamo;
DELIMITER //
CREATE PROCEDURE sp_InsertReclamo (
    IN p_fecha_reclamo DATETIME,
    IN p_descripcion VARCHAR(80),
    IN p_estado_reclamo VARCHAR(20),
    IN p_motivo_resolucion VARCHAR(70),
    IN p_fecha_resolucion DATE,
    IN p_id_usuario INT,
    IN p_id_reserva INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reclamo (fecha_reclamo, descripcion, estado_reclamo, motivo_resolucion, fecha_resolucion, id_usuario, id_reserva)
    VALUES (p_fecha_reclamo, p_descripcion, p_estado_reclamo, p_motivo_resolucion, p_fecha_resolucion, p_id_usuario, p_id_reserva);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_UpdateReclamo;
DELIMITER //
CREATE PROCEDURE sp_UpdateReclamo (
    IN p_id_reclamo INT,
    IN p_fecha_reclamo DATETIME,
    IN p_descripcion VARCHAR(80),
    IN p_estado_reclamo VARCHAR(20),
    IN p_motivo_resolucion VARCHAR(70),
    IN p_fecha_resolucion DATE,
    IN p_id_usuario INT,
    IN p_id_reserva INT
)
BEGIN
    UPDATE Reclamo
    SET 
        fecha_reclamo = p_fecha_reclamo,
        descripcion = p_descripcion,
        estado_reclamo = p_estado_reclamo,
        motivo_resolucion = p_motivo_resolucion,
        fecha_resolucion = p_fecha_resolucion,
        id_usuario = p_id_usuario,
        id_reserva = p_id_reserva
    WHERE id_reclamo = p_id_reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteReclamo;
DELIMITER //
CREATE PROCEDURE sp_DeleteReclamo (
    IN p_id_reclamo INT
)
BEGIN
    DELETE FROM Reclamo
    WHERE id_reclamo = p_id_reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListReclamos;
DELIMITER //
CREATE PROCEDURE spListReclamos()
BEGIN
    SELECT * FROM Reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListReclamoById;
DELIMITER //
CREATE PROCEDURE sp_ListReclamoById (
    IN p_id_reclamo INT
)
BEGIN
    SELECT * FROM Reclamo
    WHERE id_reclamo = p_id_reclamo;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Reporte_Ventas
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertReporte_Ventas;
DELIMITER //
CREATE PROCEDURE sp_InsertReporte_Ventas (
    IN p_fecha_generacion DATE,
    IN p_fecha_inicio_filtro DATE,
    IN p_fecha_fin_filtro DATE,
    IN p_cantidad_registros INT,
    IN p_monto_total DECIMAL(14, 2),
    IN p_id_usuario INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reporte_Ventas (fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_registros, monto_total, id_usuario)
    VALUES (p_fecha_generacion, p_fecha_inicio_filtro, p_fecha_fin_filtro, p_cantidad_registros, p_monto_total, p_id_usuario);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteReporte_Ventas;
DELIMITER //
CREATE PROCEDURE sp_DeleteReporte_Ventas (
    IN p_id_reporte_ventas INT
)
BEGIN
    DELETE FROM Reporte_Ventas
    WHERE id_reporte_ventas = p_id_reporte_ventas;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListReporteVentas;
DELIMITER //
CREATE PROCEDURE spListReporteVentas()
BEGIN
    SELECT * FROM Reporte_Ventas;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListReporteVentasById;
DELIMITER //
CREATE PROCEDURE sp_ListReporteVentasById (
    IN p_id_reporte_ventas INT
)
BEGIN
    SELECT * FROM Reporte_Ventas
    WHERE id_reporte_ventas = p_id_reporte_ventas;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Detalle_Reporte_Ventas
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertDetalle_Reporte_Ventas;
DELIMITER //
CREATE PROCEDURE sp_InsertDetalle_Reporte_Ventas (
    IN p_id_reporte_ventas INT,
    IN p_id_reservas INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reporte_Ventas (id_reporte_ventas, id_reservas)
    VALUES (p_id_reporte_ventas, p_id_reservas);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteDetalle_Reporte_Ventas;
DELIMITER //
CREATE PROCEDURE sp_DeleteDetalle_Reporte_Ventas (
    IN p_id_detalle_reporte_ventas INT
)
BEGIN
    DELETE FROM Detalle_Reporte_Ventas
    WHERE id_detalle_reporte_ventas = p_id_detalle_reporte_ventas;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListDetalleReporteVentas;
DELIMITER //
CREATE PROCEDURE spListDetalleReporteVentas()
BEGIN
    SELECT * FROM Detalle_Reporte_Ventas;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListDetalleReporteVentasById;
DELIMITER //
CREATE PROCEDURE sp_ListDetalleReporteVentasById (
    IN p_id_detalle_reporte_ventas INT
)
BEGIN
    SELECT * FROM Detalle_Reporte_Ventas
    WHERE id_detalle_reporte_ventas = p_id_detalle_reporte_ventas;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Reporte_Reclamo
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertReporte_Reclamo;
DELIMITER //
CREATE PROCEDURE sp_InsertReporte_Reclamo (
    IN p_fecha_generacion DATE,
    IN p_fecha_inicio_filtro DATE,
    IN p_fecha_fin_filtro DATE,
    IN p_cantidad_reservas INT,
    IN p_cantidad_reclamos INT,
    IN p_porcentaje_incidencias DECIMAL(5, 2),
    IN p_total_procede INT,
    IN p_total_no_procede INT,
    IN p_total_pendientes INT,
    IN p_id_usuario INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reporte_Reclamo (fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_reservas, cantidad_reclamos, porcentaje_incidencias, total_procede, total_no_procede, total_pendientes, id_usuario)
    VALUES (p_fecha_generacion, p_fecha_inicio_filtro, p_fecha_fin_filtro, p_cantidad_reservas, p_cantidad_reclamos, p_porcentaje_incidencias, p_total_procede, p_total_no_procede, p_total_pendientes, p_id_usuario);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteReporte_Reclamo;
DELIMITER //
CREATE PROCEDURE sp_DeleteReporte_Reclamo (
    IN p_id_reporte_reclamo INT
)
BEGIN
    DELETE FROM Reporte_Reclamo
    WHERE id_reporte_reclamo = p_id_reporte_reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListReporteReclamo;
DELIMITER //
CREATE PROCEDURE spListReporteReclamo()
BEGIN
    SELECT * FROM Reporte_Reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListReporteReclamoById;
DELIMITER //
CREATE PROCEDURE sp_ListReporteReclamoById (
    IN p_id_reporte_reclamo INT
)
BEGIN
    SELECT * FROM Reporte_Reclamo
    WHERE id_reporte_reclamo = p_id_reporte_reclamo;
END //
DELIMITER ;

-- ==========================================
-- CRUD para la tabla: Detalle_Reporte_Reclamo
-- ==========================================

DROP PROCEDURE IF EXISTS sp_InsertDetalle_Reporte_Reclamo;
DELIMITER //
CREATE PROCEDURE sp_InsertDetalle_Reporte_Reclamo (
    IN p_id_reporte_reclamo INT,
    IN p_id_reclamo INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reporte_Reclamo (id_reporte_reclamo, id_reclamo)
    VALUES (p_id_reporte_reclamo, p_id_reclamo);
    
    SET _id_generado = LAST_INSERT_ID();
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_DeleteDetalle_Reporte_Reclamo;
DELIMITER //
CREATE PROCEDURE sp_DeleteDetalle_Reporte_Reclamo (
    IN p_id_detalle_reporte_reclamo INT
)
BEGIN
    DELETE FROM Detalle_Reporte_Reclamo
    WHERE id_detalle_reporte_reclamo = p_id_detalle_reporte_reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS spListDetalleReporteReclamo;
DELIMITER //
CREATE PROCEDURE spListDetalleReporteReclamo()
BEGIN
    SELECT * FROM Detalle_Reporte_Reclamo;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_ListDetalleReporteReclamoById;
DELIMITER //
CREATE PROCEDURE sp_ListDetalleReporteReclamoById (
    IN p_id_detalle_reporte_reclamo INT
)
BEGIN
    SELECT * FROM Detalle_Reporte_Reclamo
    WHERE id_detalle_reporte_reclamo = p_id_detalle_reporte_reclamo;
END //
DELIMITER ;

-- ==========================================
-- Integracion Bokun: dispersa webhook completo
-- ==========================================

DROP PROCEDURE IF EXISTS sp_ProcesarYDispersarWebhookBokun;
DELIMITER //
CREATE PROCEDURE sp_ProcesarYDispersarWebhookBokun(
    IN p_raw_json LONGTEXT
)
BEGIN
    DECLARE v_cli_nombres VARCHAR(50);
    DECLARE v_cli_apellidos VARCHAR(50);
    DECLARE v_cli_correo VARCHAR(80);
    DECLARE v_cli_documento VARCHAR(20);
    DECLARE v_cli_nacionalidad VARCHAR(30);
    DECLARE v_cli_contacto VARCHAR(20);
    DECLARE v_id_cliente INT DEFAULT NULL;

    DECLARE v_usr_nombres VARCHAR(50);
    DECLARE v_usr_apellidos VARCHAR(50);
    DECLARE v_usr_correo VARCHAR(35);
    DECLARE v_id_usuario INT DEFAULT NULL;

    DECLARE v_res_bookingId VARCHAR(80);
    DECLARE v_res_fecha_reg DATETIME;
    DECLARE v_res_estado_raw VARCHAR(30);
    DECLARE v_res_estado VARCHAR(15);
    DECLARE v_res_cant_boletos INT DEFAULT 1;
    DECLARE v_res_monto_total DECIMAL(12, 2) DEFAULT 0;
    DECLARE v_res_fecha_modif DATETIME;
    DECLARE v_res_canal VARCHAR(50);
    DECLARE v_res_impuestos DECIMAL(12, 2) DEFAULT 0;
    DECLARE v_res_codigo_bokun VARCHAR(80);
    DECLARE v_id_reserva INT DEFAULT NULL;

    DECLARE v_array_len INT DEFAULT 0;
    DECLARE i INT DEFAULT 0;

    DECLARE v_srv_nombre VARCHAR(50);
    DECLARE v_srv_descripcion VARCHAR(80);
    DECLARE v_srv_precio DECIMAL(10, 2);
    DECLARE v_srv_duracion DECIMAL(5, 2);
    DECLARE v_srv_idioma VARCHAR(35);
    DECLARE v_srv_capacidad INT;
    DECLARE v_srv_recojo CHAR(1);
    DECLARE v_srv_destino VARCHAR(40);
    DECLARE v_id_servicio INT DEFAULT NULL;

    DECLARE v_det_cantidad INT;
    DECLARE v_det_subtotal DECIMAL(12, 2);
    DECLARE v_id_detalle INT;
    DECLARE v_id_log_webhook INT;

    SET v_res_bookingId = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.bookingId')), 'null');
    IF v_res_bookingId IS NULL OR v_res_bookingId = '' THEN
        SIGNAL SQLSTATE '45000'
            SET MESSAGE_TEXT = 'El JSON de Bokun no contiene bookingId';
    END IF;

    SET v_res_codigo_bokun = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.confirmationCode')), 'null');
    IF v_res_codigo_bokun IS NULL OR v_res_codigo_bokun = '' THEN
        SET v_res_codigo_bokun = v_res_bookingId;
    END IF;
    SET v_res_codigo_bokun = SUBSTRING(v_res_codigo_bokun, 1, 80);

    -- Cliente
    SET v_cli_correo = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.email')), 'null');
    IF v_cli_correo IS NULL OR v_cli_correo = '' THEN
        SET v_cli_correo = CONCAT('bokun_', SUBSTRING(v_res_bookingId, 1, 40), '@no-email.local');
    END IF;
    SET v_cli_correo = SUBSTRING(v_cli_correo, 1, 80);

    SET v_cli_nombres = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.firstName')), 'null');
    SET v_cli_apellidos = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.lastName')), 'null');
    SET v_cli_documento = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.personalIdNumber')), 'null');
    SET v_cli_nacionalidad = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.nationality')), 'null');
    SET v_cli_contacto = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.customer.phoneNumber')), 'null');

    IF v_cli_nombres IS NULL OR v_cli_nombres = '' THEN SET v_cli_nombres = 'CLIENTE'; END IF;
    IF v_cli_apellidos IS NULL OR v_cli_apellidos = '' THEN SET v_cli_apellidos = 'BOKUN'; END IF;
    IF v_cli_documento IS NULL OR v_cli_documento = '' THEN SET v_cli_documento = CONCAT('BK', SUBSTRING(v_res_bookingId, 1, 18)); END IF;
    IF v_cli_nacionalidad IS NULL OR v_cli_nacionalidad = '' THEN SET v_cli_nacionalidad = 'PE'; END IF;
    IF v_cli_contacto IS NULL OR v_cli_contacto = '' THEN SET v_cli_contacto = '00000000'; END IF;

    SELECT id_cliente INTO v_id_cliente
    FROM Cliente
    WHERE correo = v_cli_correo
       OR (tipo_documento = 'OTROS' AND numero_documento = SUBSTRING(v_cli_documento, 1, 20))
    LIMIT 1;

    IF v_id_cliente IS NULL THEN
        CALL sp_InsertCliente(
            SUBSTRING(v_cli_nombres, 1, 50),
            SUBSTRING(v_cli_apellidos, 1, 50),
            'OTROS',
            SUBSTRING(v_cli_documento, 1, 20),
            v_cli_correo,
            SUBSTRING(v_cli_nacionalidad, 1, 30),
            CURDATE(),
            SUBSTRING(v_cli_contacto, 1, 20),
            '2000-01-01',
            v_id_cliente
        );
    END IF;

    -- Usuario operador de Bokun, si viene en el JSON.
    SET v_usr_correo = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.username')), 'null');
    SET v_usr_nombres = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.firstName')), 'null');
    SET v_usr_apellidos = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.extranetUser.lastName')), 'null');

    IF v_usr_correo IS NOT NULL AND v_usr_correo <> '' THEN
        SET v_usr_correo = SUBSTRING(v_usr_correo, 1, 35);
        IF v_usr_nombres IS NULL OR v_usr_nombres = '' THEN SET v_usr_nombres = 'USUARIO'; END IF;
        IF v_usr_apellidos IS NULL OR v_usr_apellidos = '' THEN SET v_usr_apellidos = 'BOKUN'; END IF;

        SELECT id_usuario INTO v_id_usuario
        FROM Usuario
        WHERE correo = v_usr_correo
        LIMIT 1;

        IF v_id_usuario IS NULL THEN
            CALL sp_InsertUsuario(
                SUBSTRING(v_usr_nombres, 1, 50),
                SUBSTRING(v_usr_apellidos, 1, 50),
                'DNI',
                CONCAT('BK', SUBSTRING(v_res_bookingId, 1, 18)),
                v_usr_correo,
                SHA2('PasswordBokun2026!', 256),
                '00000000',
                'EXTRANET_USER',
                v_id_usuario
            );
        END IF;
    END IF;

    -- Reserva
    SET v_res_fecha_reg = FROM_UNIXTIME(COALESCE(JSON_EXTRACT(p_raw_json, '$.creationDate'), UNIX_TIMESTAMP() * 1000) / 1000);
    SET v_res_estado_raw = UPPER(NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.status')), 'null'));
    SET v_res_estado = CASE
        WHEN v_res_estado_raw IN ('CONFIRMED', 'ARRIVED', 'COMPLETED') THEN 'APROBADO'
        WHEN v_res_estado_raw IN ('CANCELLED', 'CANCELED', 'EXPIRED', 'REJECTED') THEN 'RECHAZADO'
        WHEN v_res_estado_raw IN ('PENDING', 'ON_HOLD', 'RESERVED') THEN 'PENDIENTE'
        ELSE 'OBSERVADO'
    END;

    SET v_res_monto_total = COALESCE(JSON_EXTRACT(p_raw_json, '$.totalPrice'), JSON_EXTRACT(p_raw_json, '$.invoice.totalAsMoney.amount'), 0);
    SET v_res_canal = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.bookingChannel.title')), 'null');
    IF v_res_canal IS NULL OR v_res_canal = '' THEN SET v_res_canal = 'Bokun'; END IF;
    SET v_res_canal = SUBSTRING(v_res_canal, 1, 50);
    SET v_res_impuestos = COALESCE(JSON_EXTRACT(p_raw_json, '$.invoice.totalTaxAsMoney.amount'), 0);

    IF JSON_EXTRACT(p_raw_json, '$.cancellationDate') IS NOT NULL
       AND JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, '$.cancellationDate')) <> 'null' THEN
        SET v_res_fecha_modif = FROM_UNIXTIME(JSON_EXTRACT(p_raw_json, '$.cancellationDate') / 1000);
    ELSE
        SET v_res_fecha_modif = NOW();
    END IF;

    SET v_res_cant_boletos = COALESCE(JSON_EXTRACT(p_raw_json, '$.activityBookings[0].totalParticipants'), 1);

    SELECT id_reserva INTO v_id_reserva
    FROM Reserva
    WHERE codigo_bokun = v_res_codigo_bokun
    LIMIT 1;

    IF v_id_reserva IS NULL THEN
        CALL sp_InsertReserva(
            v_res_fecha_reg,
            v_res_estado,
            v_res_cant_boletos,
            v_res_monto_total,
            v_res_fecha_modif,
            v_res_canal,
            v_res_impuestos,
            v_res_codigo_bokun,
            v_id_usuario,
            v_id_cliente,
            v_id_reserva
        );
    ELSE
        CALL sp_UpdateReserva(
            v_id_reserva,
            v_res_fecha_reg,
            v_res_estado,
            v_res_cant_boletos,
            v_res_monto_total,
            v_res_fecha_modif,
            v_res_canal,
            v_res_impuestos,
            v_res_codigo_bokun,
            v_id_usuario,
            v_id_cliente
        );

        DELETE FROM Detalle_Reserva
        WHERE id_reserva = v_id_reserva;
    END IF;

    -- Detalles
    SET v_array_len = COALESCE(JSON_LENGTH(JSON_EXTRACT(p_raw_json, '$.activityBookings')), 0);

    WHILE i < v_array_len DO
        SET v_id_servicio = NULL;
        SET v_srv_nombre = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].title'))), 'null');
        IF v_srv_nombre IS NULL OR v_srv_nombre = '' THEN
            SET v_srv_nombre = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].product.title'))), 'null');
        END IF;
        IF v_srv_nombre IS NULL OR v_srv_nombre = '' THEN SET v_srv_nombre = CONCAT('Servicio Bokun ', v_res_bookingId); END IF;

        SET v_srv_descripcion = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].product.excerpt'))), 'null');
        IF v_srv_descripcion IS NULL OR v_srv_descripcion = '' THEN
            SET v_srv_descripcion = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.excerpt'))), 'null');
        END IF;
        IF v_srv_descripcion IS NULL OR v_srv_descripcion = '' THEN SET v_srv_descripcion = 'Servicio importado desde Bokun'; END IF;

        SET v_srv_precio = COALESCE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalPrice')), 0);
        SET v_srv_duracion = COALESCE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.durationHours')), 0);
        SET v_srv_idioma = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.languages[0]'))), 'null');
        IF v_srv_idioma IS NULL OR v_srv_idioma = '' THEN SET v_srv_idioma = 'N/D'; END IF;
        SET v_srv_capacidad = COALESCE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.passCapacity')), 1);

        IF LOWER(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].pickup')))) = 'true' THEN
            SET v_srv_recojo = 'Y';
        ELSE
            SET v_srv_recojo = 'N';
        END IF;

        SET v_srv_destino = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.actualVendor.timeZone'))), 'null');
        IF v_srv_destino IS NULL OR v_srv_destino = '' THEN
            SET v_srv_destino = NULLIF(JSON_UNQUOTE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].activity.vendor.timeZone'))), 'null');
        END IF;
        IF v_srv_destino IS NULL OR v_srv_destino = '' THEN SET v_srv_destino = 'America/Lima'; END IF;

        SET v_srv_nombre = SUBSTRING(v_srv_nombre, 1, 50);
        SET v_srv_descripcion = SUBSTRING(v_srv_descripcion, 1, 80);
        SET v_srv_idioma = SUBSTRING(v_srv_idioma, 1, 35);
        SET v_srv_destino = SUBSTRING(v_srv_destino, 1, 40);

        SELECT id_servicio INTO v_id_servicio
        FROM Servicio
        WHERE nombre = v_srv_nombre
        LIMIT 1;

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
                v_id_servicio
            );
        END IF;

        SET v_det_cantidad = COALESCE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalParticipants')), 1);
        SET v_det_subtotal = COALESCE(JSON_EXTRACT(p_raw_json, CONCAT('$.activityBookings[', i, '].totalPrice')), 0);

        CALL sp_InsertDetalle_Reserva(
            v_id_reserva,
            v_id_servicio,
            v_det_cantidad,
            v_det_subtotal,
            v_id_detalle
        );

        SET i = i + 1;
    END WHILE;

    CALL sp_InsertWebhookLog(
        v_res_bookingId,
        NOW(),
        p_raw_json,
        v_id_reserva,
        v_id_log_webhook
    );
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS sp_LoginUsuario;
DELIMITER //
CREATE PROCEDURE sp_LoginUsuario (
    IN p_correo VARCHAR(35),
    IN p_contrasena VARCHAR(80),
    IN p_tipo_usuario VARCHAR(50),
    OUT p_valido BOOLEAN
)
BEGIN
    DECLARE v_contrasena_guardada VARCHAR(80) DEFAULT NULL;
    
    SELECT contrasena INTO v_contrasena_guardada
    FROM Usuario
    WHERE correo = p_correo
      AND tipo_usuario = p_tipo_usuario
    LIMIT 1;
    
    IF v_contrasena_guardada IS NOT NULL AND 
       (v_contrasena_guardada = p_contrasena OR v_contrasena_guardada = SHA2(p_contrasena, 256)) THEN
        SET p_valido = TRUE;
    ELSE
        SET p_valido = FALSE;
    END IF;
END //
DELIMITER ;

