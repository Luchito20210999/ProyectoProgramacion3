-- =============================================================================
-- SCRIPT DE CREACIÓN DE TABLAS (DDL)
-- Esquema: SACRSoft
-- Dialecto: MySQL
-- =============================================================================

CREATE SCHEMA IF NOT EXISTS SACRSoft;
USE SACRSoft;

-- =============================================================================
-- 1. ELIMINACIÓN DE TABLAS (DROPS en orden inverso para evitar errores de FK)
-- =============================================================================
DROP TABLE IF EXISTS Detalle_Reporte_Reclamo;
DROP TABLE IF EXISTS Detalle_Reporte_Ventas;
DROP TABLE IF EXISTS Reporte_Reclamo;
DROP TABLE IF EXISTS Reporte_Ventas;
DROP TABLE IF EXISTS Reclamo;
DROP TABLE IF EXISTS WebhookLog;
DROP TABLE IF EXISTS Detalle_Reserva;
DROP TABLE IF EXISTS Reserva;
DROP TABLE IF EXISTS Notificacion;
DROP TABLE IF EXISTS Log_Auditoria;
DROP TABLE IF EXISTS Servicio;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Usuario;

-- =============================================================================
-- 2. CREACIÓN DE TABLAS
-- =============================================================================

-- 1. TABLA: Usuario
CREATE TABLE Usuario (
    id_usuario INT NOT NULL AUTO_INCREMENT,
    nombres VARCHAR(50) NOT NULL,
    apellidos VARCHAR(50) NOT NULL,
    tipo_documento VARCHAR(25) NOT NULL,
    numero_documento VARCHAR(20) NOT NULL,
    correo VARCHAR(35) NOT NULL,
    contrasena VARCHAR(80) NOT NULL,
    numero_contacto VARCHAR(20),
    tipo_usuario VARCHAR(50) NOT NULL,
    activo BOOLEAN NOT NULL DEFAULT TRUE, 
    CONSTRAINT PK_Usuario PRIMARY KEY (id_usuario),
    CONSTRAINT UQ_Usuario_Correo UNIQUE (correo)
);

-- 2. TABLA: Cliente
CREATE TABLE Cliente (
    id_cliente INT NOT NULL AUTO_INCREMENT,
    nombres VARCHAR(50) NOT NULL,
    apellidos VARCHAR(50) NOT NULL,
    tipo_documento VARCHAR(25) NOT NULL,
    numero_documento VARCHAR(20) NOT NULL,
    correo VARCHAR(80) NOT NULL,
    nacionalidad VARCHAR(30),
    fecha_registro DATE NOT NULL,
    numero_contacto VARCHAR(20),
    fecha_nacimiento DATE,
    activo BOOLEAN NOT NULL DEFAULT TRUE, 
    CONSTRAINT PK_Cliente PRIMARY KEY (id_cliente),
    CONSTRAINT UQ_Cliente_Doc UNIQUE (tipo_documento, numero_documento)
);

-- 3. TABLA: Servicio
CREATE TABLE Servicio (
    id_servicio INT NOT NULL AUTO_INCREMENT,
    nombre VARCHAR(50) NOT NULL,
    descripcion VARCHAR(80),
    precio_usd DECIMAL(10, 2) NOT NULL,
    duracion_horas DECIMAL(5, 2),
    idioma_guia VARCHAR(35),
    capacidad_maxima INT,
    incluye_recojo CHAR(1) DEFAULT 'N',
    ciudad_destino VARCHAR(40),
    activo BOOLEAN NOT NULL DEFAULT TRUE, 
    CONSTRAINT PK_Servicio PRIMARY KEY (id_servicio),
    CONSTRAINT CHK_Incluye_Recojo CHECK (incluye_recojo IN ('Y', 'N'))
);

-- 4. TABLA: Log_Auditoria
CREATE TABLE Log_Auditoria (
    idLogAuditoria INT NOT NULL AUTO_INCREMENT,
    descripcion VARCHAR(150) NOT NULL,
    accion VARCHAR(70) NOT NULL,
    fecha_registro DATETIME NOT NULL,
    origenAccion VARCHAR(150),
    id_usuario INT NOT NULL,
    CONSTRAINT PK_Log_Auditoria PRIMARY KEY (idLogAuditoria),
    CONSTRAINT FK_LogAuditoria_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario)
);

-- 5. TABLA: Notificacion
CREATE TABLE Notificacion (
    id_notificacion INT NOT NULL AUTO_INCREMENT,
    mensaje VARCHAR(150) NOT NULL,
    tipo_notificacion VARCHAR(70) NOT NULL,
    fecha_envio DATETIME NOT NULL,
    leido CHAR(1) DEFAULT 'N',
    id_usuario INT NOT NULL,
    CONSTRAINT PK_Notificacion PRIMARY KEY (id_notificacion),
    CONSTRAINT FK_Notificacion_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario),
    CONSTRAINT CHK_Notificacion_Leido CHECK (leido IN ('Y', 'N'))
);

-- 6. TABLA: Reserva
CREATE TABLE Reserva (
    id_reserva INT NOT NULL AUTO_INCREMENT,
    fecha_registro DATETIME NOT NULL,
    estado_reserva VARCHAR(15) NOT NULL,
    cantidad_boletos INT NOT NULL,
    monto_total DECIMAL(12, 2) NOT NULL,
    fecha_ultima_modif DATETIME,
    canal_venta VARCHAR(50),
    monto_impuestos DECIMAL(12, 2),
    codigo_bokun VARCHAR(80),
    id_usuario INT,
    id_cliente INT NOT NULL,
    CONSTRAINT PK_Reserva PRIMARY KEY (id_reserva),
    CONSTRAINT FK_Reserva_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario),
    CONSTRAINT FK_Reserva_Cliente FOREIGN KEY (id_cliente) 
        REFERENCES Cliente(id_cliente),
    CONSTRAINT CHK_Estado_Reserva CHECK (estado_reserva IN ('APROBADO', 'PENDIENTE', 'RECHAZADO', 'OBSERVADO'))
);

-- 7. TABLA: Detalle_Reserva
CREATE TABLE Detalle_Reserva (
    id_detalle_reserva INT NOT NULL AUTO_INCREMENT,
    id_reserva INT NOT NULL,
    id_servicio INT NOT NULL,
    cantidad INT NOT NULL,
    subtotal DECIMAL(12, 2) NOT NULL,
    CONSTRAINT PK_Detalle_Reserva PRIMARY KEY (id_detalle_reserva),
    CONSTRAINT FK_DetalleReserva_Reserva FOREIGN KEY (id_reserva) 
        REFERENCES Reserva(id_reserva) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleReserva_Servicio FOREIGN KEY (id_servicio) 
        REFERENCES Servicio(id_servicio)
);

-- 8. TABLA: WebhookLog
CREATE TABLE WebhookLog (
    id_log INT NOT NULL AUTO_INCREMENT,
    bookingId VARCHAR(80) NOT NULL,
    timestamp DATETIME NOT NULL,
    raw_data LONGTEXT,
    id_reserva INT,
    CONSTRAINT PK_WebhookLog PRIMARY KEY (id_log),
    CONSTRAINT FK_WebhookLog_Reserva FOREIGN KEY (id_reserva) 
        REFERENCES Reserva(id_reserva)
);

-- 9. TABLA: Reclamo
CREATE TABLE Reclamo (
    id_reclamo INT NOT NULL AUTO_INCREMENT,
    fecha_reclamo DATETIME NOT NULL,
    descripcion VARCHAR(80) NOT NULL,
    estado_reclamo VARCHAR(20) NOT NULL,
    motivo_resolucion VARCHAR(70),
    fecha_resolucion DATE,
    id_usuario INT,
    id_reserva INT NOT NULL,
    CONSTRAINT PK_Reclamo PRIMARY KEY (id_reclamo),
    CONSTRAINT FK_Reclamo_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario),
    CONSTRAINT FK_Reclamo_Reserva FOREIGN KEY (id_reserva) 
        REFERENCES Reserva(id_reserva),
    CONSTRAINT CHK_Estado_Reclamo CHECK (estado_reclamo IN ('PENDIENTE', 'PROCEDE', 'NO_PROCEDE', 'EN_ATENCION'))
);

-- 10. TABLA: Reporte_Ventas
CREATE TABLE Reporte_Ventas (
    id_reporte_ventas INT NOT NULL AUTO_INCREMENT,
    fecha_generacion DATE NOT NULL,
    fecha_inicio_filtro DATE NOT NULL,
    fecha_fin_filtro DATE NOT NULL,
    cantidad_registros INT NOT NULL,
    monto_total DECIMAL(14, 2) NOT NULL,
    id_usuario INT NOT NULL,
    CONSTRAINT PK_Reporte_Ventas PRIMARY KEY (id_reporte_ventas),
    CONSTRAINT FK_ReporteVentas_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario)
);

-- 11. TABLA: Detalle_Reporte_Ventas
CREATE TABLE Detalle_Reporte_Ventas (
    id_detalle_reporte_ventas INT NOT NULL AUTO_INCREMENT,
    id_reporte_ventas INT NOT NULL,
    id_reservas INT NOT NULL,
    CONSTRAINT PK_Detalle_Reporte_Ventas PRIMARY KEY (id_detalle_reporte_ventas),
    CONSTRAINT FK_DetalleVentas_Reporte FOREIGN KEY (id_reporte_ventas) 
        REFERENCES Reporte_Ventas(id_reporte_ventas) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleVentas_Reserva FOREIGN KEY (id_reservas) 
        REFERENCES Reserva(id_reserva)
);

-- 12. TABLA: Reporte_Reclamo
CREATE TABLE Reporte_Reclamo (
    id_reporte_reclamo INT NOT NULL AUTO_INCREMENT,
    fecha_generacion DATE NOT NULL,
    fecha_inicio_filtro DATE NOT NULL,
    fecha_fin_filtro DATE NOT NULL,
    cantidad_reservas INT NOT NULL,
    cantidad_reclamos INT NOT NULL,
    porcentaje_incidencias DECIMAL(5, 2),
    total_procede INT NOT NULL,
    total_no_procede INT NOT NULL,
    total_pendientes INT NOT NULL,
    id_usuario INT NOT NULL,
    CONSTRAINT PK_Reporte_Reclamo PRIMARY KEY (id_reporte_reclamo),
    CONSTRAINT FK_ReporteReclamo_Usuario FOREIGN KEY (id_usuario) 
        REFERENCES Usuario(id_usuario)
);

-- 13. TABLA: Detalle_Reporte_Reclamo
CREATE TABLE Detalle_Reporte_Reclamo (
    id_detalle_reporte_reclamo INT NOT NULL AUTO_INCREMENT,
    id_reporte_reclamo INT NOT NULL,
    id_reclamo INT NOT NULL,
    CONSTRAINT PK_Detalle_Reporte_Reclamo PRIMARY KEY (id_detalle_reporte_reclamo),
    CONSTRAINT FK_DetalleReclamos_Reporte FOREIGN KEY (id_reporte_reclamo) 
        REFERENCES Reporte_Reclamo(id_reporte_reclamo) ON DELETE CASCADE,
    CONSTRAINT FK_DetalleReclamos_Reclamo FOREIGN KEY (id_reclamo) 
        REFERENCES Reclamo(id_reclamo)
);