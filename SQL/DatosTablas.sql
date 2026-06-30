-- =============================================================================
-- SCRIPT DE POBLAMIENTO DE DATOS (SEED)
-- Esquema: SACRSoft
-- Dialecto: MySQL
-- =============================================================================

USE SACRSoft;

-- (Opcional) Limpiar las tablas antes de insertar para evitar duplicados si lo corres varias veces
SET FOREIGN_KEY_CHECKS = 0;
TRUNCATE TABLE Detalle_Reporte_Reclamo;
TRUNCATE TABLE Detalle_Reporte_Ventas;
TRUNCATE TABLE Reporte_Reclamo;
TRUNCATE TABLE Reporte_Ventas;
TRUNCATE TABLE Reclamo;
TRUNCATE TABLE WebhookLog;
TRUNCATE TABLE Detalle_Reserva;
TRUNCATE TABLE Reserva;
TRUNCATE TABLE Notificacion;
TRUNCATE TABLE Log_Auditoria;
TRUNCATE TABLE Servicio;
TRUNCATE TABLE Cliente;
TRUNCATE TABLE Usuario;
SET FOREIGN_KEY_CHECKS = 1;

-- =============================================================================
-- 1. TABLAS MAESTRAS (Sin dependencias)
-- =============================================================================

-- Insertar Usuarios (Se proveen IDs explícitos para mantener la integridad en el script)
INSERT INTO Usuario (id_usuario, nombres, apellidos, tipo_documento, numero_documento, correo, contrasena, numero_contacto, tipo_usuario, activo) VALUES 
(1, 'Carlos', 'Administrador', 'DNI', '11111111', 'admin@sacrsoft.com', '$10$.PQ4n8YRQr/mt73sg2cVq.a4Tkgvm7jCAfuaSpXFfzvrQsQlkmgBy', '987654321', 'Administrador', TRUE),
(2, 'Maria', 'Operadora', 'DNI', '22222222', 'operador1@sacrsoft.com', '$10$qnfqQZdk2yDUQv1ykBX1CeqTitivukZoo9MdTpQJRHi7Guirp/AU.', '987654322', 'Operador', TRUE),
(3, 'Jorge', 'Analista', 'CARNET_DE_EXTRANJERIA', '33333333', 'analista1@sacrsoft.com', '$10$kBc3NV0zEWGleuLZHOTmSOG4UCOpjT9.Gt.2HZL07spSTVxgLK8Ty', '987654323', 'Analista', TRUE);

-- Insertar Clientes
INSERT INTO Cliente (id_cliente, nombres, apellidos, tipo_documento, numero_documento, correo, nacionalidad, fecha_registro, numero_contacto, fecha_nacimiento, activo) VALUES 
(1, 'John', 'Doe', 'PASAPORTE', 'US987654', 'johndoe@email.com', 'Estadounidense', '2023-10-01', '+1 555-0198', '1985-06-15', TRUE),
(2, 'Lucia', 'Mendez', 'DNI', '76543210', 'lucia.mendez@email.com', 'Peruana', '2023-10-05', '+51 999888777', '1990-11-20', TRUE);

-- Insertar Servicios
INSERT INTO Servicio (id_servicio, nombre, descripcion, precio_usd, duracion_horas, idioma_guia, capacidad_maxima, incluye_recojo, ciudad_destino, activo) VALUES 
(1, 'City Tour Gastronómico', 'Recorrido por el centro histórico con cata de pisco', 45.50, 4.00, 'Inglés/Español', 15, 'Y', 'Lima', TRUE),
(2, 'Trekking Montaña 7 Colores', 'Excursión de día completo a Vinicunca', 120.00, 14.00, 'Inglés/Español', 10, 'Y', 'Cusco', TRUE),
(3, 'Tour Ruinas Locales', 'Visita guiada por sitio arqueológico', 25.00, 2.50, 'Español', 20, 'N', 'Arequipa', TRUE);

-- =============================================================================
-- 2. TABLAS TRANSACCIONALES (Dependen de Maestras)
-- =============================================================================

-- Insertar Log Auditoria
INSERT INTO Log_Auditoria (idLogAuditoria, descripcion, accion, fecha_registro, origenAccion, id_usuario) VALUES 
(1, 'Creación de nuevo servicio City Tour', 'CREAR_SERVICIO', '2023-10-10 08:30:00', 'Módulo de Servicios', 1),
(2, 'Actualización de tarifa Tour Ruinas', 'ACTUALIZAR_SERVICIO', '2023-10-10 09:15:00', 'Módulo de Servicios', 1);

-- Insertar Notificaciones
INSERT INTO Notificacion (id_notificacion, mensaje, tipo_notificacion, fecha_envio, leido, id_usuario) VALUES 
(1, 'Tiene una nueva reserva asignada', 'NUEVA_RESERVA', '2023-10-15 10:00:00', 'Y', 2),
(2, 'Reclamo #1 pendiente de revisión', 'RECLAMO_PENDIENTE', '2023-10-16 11:20:00', 'N', 2);

-- Insertar Reservas
INSERT INTO Reserva (id_reserva, fecha_registro, estado_reserva, cantidad_boletos, monto_total, fecha_ultima_modif, canal_venta, monto_impuestos, codigo_bokun, id_usuario, id_cliente) VALUES 
(1, '2023-10-15 09:45:00', 'APROBADO', 2, 91.00, '2023-10-15 09:50:00', 'Bokun', 16.38, 'BOKUN-REF-001', 2, 1),
(2, '2023-10-18 14:20:00', 'PENDIENTE', 1, 120.00, NULL, 'Web Directa', 21.60, NULL, 2, 2);

-- Insertar Detalle Reservas
INSERT INTO Detalle_Reserva (id_detalle_reserva, id_reserva, id_servicio, cantidad, subtotal) VALUES 
(1, 1, 1, 2, 91.00),
(2, 2, 2, 1, 120.00);

-- Insertar WebhookLog
INSERT INTO WebhookLog (id_log, bookingId, timestamp, raw_data, id_reserva) VALUES 
(1, 'BOKUN-REF-001', '2023-10-15 09:45:05', '{"event": "booking.created", "customer": "John Doe", "amount": 91.0}', 1);

-- Insertar Reclamos
INSERT INTO Reclamo (id_reclamo, fecha_reclamo, descripcion, estado_reclamo, motivo_resolucion, fecha_resolucion, id_usuario, id_reserva) VALUES 
(1, '2023-10-16 11:00:00', 'El transporte de recojo llegó con 30 minutos de retraso', 'EN_ATENCION', NULL, NULL, 2, 1),
(2, '2023-10-10 15:00:00', 'Diferencia en el cobro de la tarifa', 'PROCEDE', 'Se aplicó reembolso del 10% por error de sistema', '2023-10-12', 2, 2);

-- =============================================================================
-- 3. TABLAS DE REPORTES (Dependen de Transaccionales)
-- =============================================================================

-- Insertar Reporte_Ventas
INSERT INTO Reporte_Ventas (id_reporte_ventas, fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_registros, monto_total, id_usuario) VALUES 
(1, '2023-10-31', '2023-10-01', '2023-10-31', 2, 211.00, 3);

-- Insertar Detalle_Reporte_Ventas
INSERT INTO Detalle_Reporte_Ventas (id_detalle_reporte_ventas, id_reporte_ventas, id_reservas) VALUES 
(1, 1, 1),
(2, 1, 2);

-- Insertar Reporte_Reclamo
INSERT INTO Reporte_Reclamo (id_reporte_reclamo, fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_reservas, cantidad_reclamos, porcentaje_incidencias, total_procede, total_no_procede, total_pendientes, id_usuario) VALUES 
(1, '2023-10-31', '2023-10-01', '2023-10-31', 2, 2, 100.00, 1, 0, 1, 3);

-- Insertar Detalle_Reporte_Reclamo
INSERT INTO Detalle_Reporte_Reclamo (id_detalle_reporte_reclamo, id_reporte_reclamo, id_reclamo) VALUES 
(1, 1, 1),
(2, 1, 2);