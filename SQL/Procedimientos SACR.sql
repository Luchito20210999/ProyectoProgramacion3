DELIMITER //

-- ==========================================
-- 1. CLIENTE
-- ==========================================
CREATE PROCEDURE sp_InsertCliente(
    IN _nom VARCHAR(50), IN _ape VARCHAR(50), IN _tipoDoc VARCHAR(50), 
    IN _numDoc VARCHAR(20), IN _corr VARCHAR(80), IN _nac VARCHAR(30), 
    IN _fReg DATE, IN _tel VARCHAR(20), IN _fNac DATE,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Cliente(nombres, apellidos, tipo_documento, numero_documento, correo, nacionalidad, fecha_registro, numero_contacto, fecha_nacimiento)
    VALUES (_nom, _ape, _tipoDoc, _numDoc, _corr, _nac, _fReg, _tel, _fNac);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateCliente(
    IN _id INT, IN _nom VARCHAR(50), IN _ape VARCHAR(50), IN _tipoDoc VARCHAR(50), 
    IN _numDoc VARCHAR(20), IN _corr VARCHAR(80), IN _nac VARCHAR(30), 
    IN _fReg DATE, IN _tel VARCHAR(20), IN _fNac DATE
)
BEGIN
    UPDATE Cliente SET nombres=_nom, apellidos=_ape, tipo_documento=_tipoDoc, numero_documento=_numDoc, correo=_corr, nacionalidad=_nac, fecha_registro=_fReg, numero_contacto=_tel, fecha_nacimiento=_fNac WHERE id_cliente=_id;
END //

CREATE PROCEDURE sp_DeleteCliente(IN _id INT) BEGIN DELETE FROM Cliente WHERE id_cliente=_id; END //
CREATE PROCEDURE sp_ListClientes() BEGIN SELECT * FROM Cliente; END //

-- ==========================================
-- 2. USUARIO
-- ==========================================
CREATE PROCEDURE sp_InsertUsuario(
    IN _nom VARCHAR(50), IN _ape VARCHAR(50), IN _tipoDoc VARCHAR(20), 
    IN _numDoc VARCHAR(20), IN _corr VARCHAR(35), IN _pass VARCHAR(80), 
    IN _tel VARCHAR(20), IN _tipoUsu VARCHAR(50),
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Usuario(nombres, apellidos, tipo_documento, numero_documento, correo, contrasena, numero_contacto, tipo_usuario)
    VALUES (_nom, _ape, _tipoDoc, _numDoc, _corr, _pass, _tel, _tipoUsu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateUsuario(
    IN _id INT, IN _nom VARCHAR(50), IN _ape VARCHAR(50), IN _tipoDoc VARCHAR(20), 
    IN _numDoc VARCHAR(20), IN _corr VARCHAR(35), IN _pass VARCHAR(80), 
    IN _tel VARCHAR(20), IN _tipoUsu VARCHAR(50)
)
BEGIN
    UPDATE Usuario SET nombres=_nom, apellidos=_ape, tipo_documento=_tipoDoc, numero_documento=_numDoc, correo=_corr, contrasena=_pass, numero_contacto=_tel, tipo_usuario=_tipoUsu WHERE id_usuario=_id;
END //

CREATE PROCEDURE sp_DeleteUsuario(IN _id INT) BEGIN DELETE FROM Usuario WHERE id_usuario=_id; END //
CREATE PROCEDURE sp_ListUsuarios() BEGIN SELECT * FROM Usuario; END //

-- ==========================================
-- 3. SERVICIO
-- ==========================================
CREATE PROCEDURE sp_InsertServicio(
    IN _nom VARCHAR(50), IN _desc VARCHAR(80), IN _pre DOUBLE, 
    IN _dur DOUBLE, IN _idioma VARCHAR(35), IN _cap INT, 
    IN _recojo BOOLEAN, IN _ciu VARCHAR(40),
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Servicio(nombre, descripcion, precio_usd, duracion_horas, idioma_guia, capacidad_maxima, incluye_recojo, ciudad_destino)
    VALUES (_nom, _desc, _pre, _dur, _idioma, _cap, _recojo, _ciu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateServicio(
    IN _id INT, IN _nom VARCHAR(50), IN _desc VARCHAR(80), IN _pre DOUBLE, 
    IN _dur DOUBLE, IN _idioma VARCHAR(35), IN _cap INT, 
    IN _recojo BOOLEAN, IN _ciu VARCHAR(40)
)
BEGIN
    UPDATE Servicio SET nombre=_nom, descripcion=_desc, precio_usd=_pre, duracion_horas=_dur, idioma_guia=_idioma, capacidad_maxima=_cap, incluye_recojo=_recojo, ciudad_destino=_ciu WHERE id_servicio=_id;
END //

CREATE PROCEDURE sp_DeleteServicio(IN _id INT) BEGIN DELETE FROM Servicio WHERE id_servicio=_id; END //
CREATE PROCEDURE sp_ListServicios() BEGIN SELECT * FROM Servicio; END //

-- ==========================================
-- 4. USUARIO_SUPERVISION
-- ==========================================
CREATE PROCEDURE sp_InsertUsuarioSupervision(
    IN _idSup INT, IN _idSub INT, IN _fAsig DATE, IN _act BOOLEAN,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Usuario_Supervision(id_usuario_superior, id_usuario_subordinado, fecha_asignacion, activo)
    VALUES (_idSup, _idSub, _fAsig, _act);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateUsuarioSupervision(
    IN _id INT, IN _idSup INT, IN _idSub INT, IN _fAsig DATE, IN _act BOOLEAN
)
BEGIN
    UPDATE Usuario_Supervision SET id_usuario_superior=_idSup, id_usuario_subordinado=_idSub, fecha_asignacion=_fAsig, activo=_act WHERE id_supervision=_id;
END //

CREATE PROCEDURE sp_DeleteUsuarioSupervision(IN _id INT) BEGIN DELETE FROM Usuario_Supervision WHERE id_supervision=_id; END //
CREATE PROCEDURE sp_ListUsuarioSupervision() BEGIN SELECT * FROM Usuario_Supervision; END //

-- ==========================================
-- 5. LOG_AUDITORIA
-- ==========================================
CREATE PROCEDURE sp_InsertLogAuditoria(
    IN _desc VARCHAR(150), IN _acc VARCHAR(70), IN _fReg DATETIME, 
    IN _ori VARCHAR(150), IN _idUsu INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Log_Auditoria(descripcion, accion, fecha_registro, origenAccion, id_usuario)
    VALUES (_desc, _acc, _fReg, _ori, _idUsu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_DeleteLogAuditoria(IN _id INT) BEGIN DELETE FROM Log_Auditoria WHERE idLogAuditoria=_id; END //
CREATE PROCEDURE sp_ListLogAuditoria() BEGIN SELECT * FROM Log_Auditoria; END //

-- ==========================================
-- 6. NOTIFICACION
-- ==========================================
CREATE PROCEDURE sp_InsertNotificacion(
    IN _msj VARCHAR(150), IN _tipo VARCHAR(70), IN _fEnv DATETIME, 
    IN _leido BOOLEAN, IN _idUsu INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Notificacion(mensaje, tipo_notificacion, fecha_envio, leido, id_usuario)
    VALUES (_msj, _tipo, _fEnv, _leido, _idUsu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateNotificacion(
    IN _id INT, IN _msj VARCHAR(150), IN _tipo VARCHAR(70), IN _fEnv DATETIME, 
    IN _leido BOOLEAN, IN _idUsu INT
)
BEGIN
    UPDATE Notificacion SET mensaje=_msj, tipo_notificacion=_tipo, fecha_envio=_fEnv, leido=_leido, id_usuario=_idUsu WHERE id_notificacion=_id;
END //

CREATE PROCEDURE sp_DeleteNotificacion(IN _id INT) BEGIN DELETE FROM Notificacion WHERE id_notificacion=_id; END //
CREATE PROCEDURE sp_ListNotificaciones() BEGIN SELECT * FROM Notificacion; END //

-- ==========================================
-- 7. RESERVA
-- ==========================================
CREATE PROCEDURE sp_InsertReserva(
    IN _fReg DATETIME, IN _est VARCHAR(15), IN _cant INT, IN _tot DOUBLE, 
    IN _fMod DATETIME, IN _can VARCHAR(50), IN _imp DOUBLE, 
    IN _codBok VARCHAR(80), IN _idUsu INT, IN _idCli INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reserva(fecha_registro, estado_reserva, cantidad_boletos, monto_total, fecha_ultima_modif, canal_venta, monto_impuestos, codigo_bokun, id_usuario, id_cliente)
    VALUES (_fReg, _est, _cant, _tot, _fMod, _can, _imp, _codBok, _idUsu, _idCli);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateReserva(
    IN _id INT, IN _fReg DATETIME, IN _est VARCHAR(15), IN _cant INT, IN _tot DOUBLE, 
    IN _fMod DATETIME, IN _can VARCHAR(50), IN _imp DOUBLE, 
    IN _codBok VARCHAR(80), IN _idUsu INT, IN _idCli INT
)
BEGIN
    UPDATE Reserva SET fecha_registro=_fReg, estado_reserva=_est, cantidad_boletos=_cant, monto_total=_tot, fecha_ultima_modif=_fMod, canal_venta=_can, monto_impuestos=_imp, codigo_bokun=_codBok, id_usuario=_idUsu, id_cliente=_idCli WHERE id_reserva=_id;
END //

CREATE PROCEDURE sp_DeleteReserva(IN _id INT) BEGIN DELETE FROM Reserva WHERE id_reserva=_id; END //
CREATE PROCEDURE sp_ListReservas() BEGIN SELECT * FROM Reserva; END //

-- ==========================================
-- 8. DETALLE_RESERVA
-- ==========================================
CREATE PROCEDURE sp_InsertDetalleReserva(
    IN _idRes INT, IN _idSer INT, IN _cant INT, IN _sub DOUBLE,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reserva(id_reserva, id_servicio, cantidad, subtotal) VALUES (_idRes, _idSer, _cant, _sub);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateDetalleReserva(
    IN _id INT, IN _idRes INT, IN _idSer INT, IN _cant INT, IN _sub DOUBLE
)
BEGIN
    UPDATE Detalle_Reserva SET id_reserva=_idRes, id_servicio=_idSer, cantidad=_cant, subtotal=_sub WHERE id_detalle_reserva=_id;
END //

CREATE PROCEDURE sp_DeleteDetalleReserva(IN _id INT) BEGIN DELETE FROM Detalle_Reserva WHERE id_detalle_reserva=_id; END //
CREATE PROCEDURE sp_ListDetalleReserva() BEGIN SELECT * FROM Detalle_Reserva; END //

-- ==========================================
-- 9. RECLAMO
-- ==========================================
CREATE PROCEDURE sp_InsertReclamo(
    IN _fRec DATETIME, IN _desc VARCHAR(80), IN _est VARCHAR(20), 
    IN _mot VARCHAR(70), IN _fRes DATE, IN _idUsu INT, IN _idRes INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reclamo(fecha_reclamo, descripcion, estado_reclamo, motivo_resolucion, fecha_resolucion, id_usuario, id_reserva)
    VALUES (_fRec, _desc, _est, _mot, _fRes, _idUsu, _idRes);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_UpdateReclamo(
    IN _id INT, IN _fRec DATETIME, IN _desc VARCHAR(80), IN _est VARCHAR(20), 
    IN _mot VARCHAR(70), IN _fRes DATE, IN _idUsu INT, IN _idRes INT
)
BEGIN
    UPDATE Reclamo SET fecha_reclamo=_fRec, descripcion=_desc, estado_reclamo=_est, motivo_resolucion=_mot, fecha_resolucion=_fRes, id_usuario=_idUsu, id_reserva=_idRes WHERE id_reclamo=_id;
END //

CREATE PROCEDURE sp_DeleteReclamo(IN _id INT) BEGIN DELETE FROM Reclamo WHERE id_reclamo=_id; END //
CREATE PROCEDURE sp_ListReclamos() BEGIN SELECT * FROM Reclamo; END //

-- ==========================================
-- 10. WEBHOOKLOG
-- ==========================================
CREATE PROCEDURE sp_InsertWebhookLog(
    IN _bookId VARCHAR(80), IN _time DATETIME, IN _raw LONGTEXT, IN _idRes INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO WebhookLog(bookingId, timestamp, raw_data, id_reserva) 
    VALUES (_bookId, _time, _raw, _idRes);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_DeleteWebhookLog(IN _id INT) BEGIN DELETE FROM WebhookLog WHERE id_log=_id; END //
CREATE PROCEDURE sp_ListWebhookLog() BEGIN SELECT * FROM WebhookLog; END //

-- ==========================================
-- 11. REPORTE_VENTAS & DETALLE
-- ==========================================
CREATE PROCEDURE sp_InsertReporteVentas(
    IN _fGen DATE, IN _fIni DATE, IN _fFin DATE, IN _cantReg INT, 
    IN _mTot DOUBLE, IN _idUsu INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reporte_Ventas(fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_registros, monto_total, id_usuario)
    VALUES (_fGen, _fIni, _fFin, _cantReg, _mTot, _idUsu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_InsertDetalleReporteVentas(
    IN _idRepVen INT, IN _idRes INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reporte_Ventas(id_reporte_ventas, id_reservas) VALUES (_idRepVen, _idRes);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_ListReporteVentas() BEGIN SELECT * FROM Reporte_Ventas; END //

-- ==========================================
-- 12. REPORTE_RECLAMO & DETALLE
-- ==========================================
CREATE PROCEDURE sp_InsertReporteReclamo(
    IN _fGen DATE, IN _fIni DATE, IN _fFin DATE, IN _cRes INT, 
    IN _cRec INT, IN _porc DOUBLE, IN _tPro INT, IN _tNoP INT, 
    IN _tPen INT, IN _idUsu INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Reporte_Reclamo(fecha_generacion, fecha_inicio_filtro, fecha_fin_filtro, cantidad_reservas, cantidad_reclamos, porcentaje_incidencias, total_procede, total_no_procede, total_pendientes, id_usuario)
    VALUES (_fGen, _fIni, _fFin, _cRes, _cRec, _porc, _tPro, _tNoP, _tPen, _idUsu);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_InsertDetalleReporteReclamo(
    IN _idRepRec INT, IN _idRec INT,
    OUT _id_generado INT
)
BEGIN
    INSERT INTO Detalle_Reporte_Reclamo(id_reporte_reclamo, id_reclamo) VALUES (_idRepRec, _idRec);
    SET _id_generado = LAST_INSERT_ID();
END //

CREATE PROCEDURE sp_ListReporteReclamo() BEGIN SELECT * FROM Reporte_Reclamo; END //

DELIMITER ;