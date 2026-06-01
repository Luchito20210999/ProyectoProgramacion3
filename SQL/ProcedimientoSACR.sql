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