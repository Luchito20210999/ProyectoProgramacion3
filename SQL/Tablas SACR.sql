-- Eliminar tablas dependientes (hijas) primero
DROP TABLE IF EXISTS Detalle_Reporte_Reclamo;
DROP TABLE IF EXISTS Detalle_Reporte_Ventas;
DROP TABLE IF EXISTS WebhookLog;
DROP TABLE IF EXISTS Reclamo;
DROP TABLE IF EXISTS Detalle_Reserva;
DROP TABLE IF EXISTS Reserva;
DROP TABLE IF EXISTS Reporte_Reclamo;
DROP TABLE IF EXISTS Reporte_Ventas;
DROP TABLE IF EXISTS Notificacion;
DROP TABLE IF EXISTS Log_Auditoria;
DROP TABLE IF EXISTS Usuario_Supervision;

-- Eliminar tablas independientes (padres) al final
DROP TABLE IF EXISTS Servicio;
DROP TABLE IF EXISTS Usuario;
DROP TABLE IF EXISTS Cliente;

CREATE TABLE Cliente (
    id_cliente INT PRIMARY KEY AUTO_INCREMENT,
    nombres VARCHAR(50),
    apellidos VARCHAR(50),
    tipo_documento VARCHAR(50),
    numero_documento VARCHAR(20),
    correo VARCHAR(80),
    nacionalidad VARCHAR(30),
    fecha_registro DATE,
    numero_contacto VARCHAR(20),
    fecha_nacimiento DATE
);

CREATE TABLE Usuario (
    id_usuario INT PRIMARY KEY AUTO_INCREMENT,
    nombres VARCHAR(50),
    apellidos VARCHAR(50),
    tipo_documento VARCHAR(20),
    numero_documento VARCHAR(20),
    correo VARCHAR(35),
    contrasena VARCHAR(80),
    numero_contacto VARCHAR(20),
    tipo_usuario VARCHAR(50)
);

CREATE TABLE Servicio (
    id_servicio INT PRIMARY KEY AUTO_INCREMENT,
    nombre VARCHAR(50),
    descripcion VARCHAR(80),
    precio_usd DOUBLE,
    duracion_horas DOUBLE,
    idioma_guia VARCHAR(35),
    capacidad_maxima INT,
    incluye_recojo BOOLEAN,
    ciudad_destino VARCHAR(40)
);

CREATE TABLE Usuario_Supervision (
    id_supervision INT PRIMARY KEY AUTO_INCREMENT,
    id_usuario_superior INT,
    id_usuario_subordinado INT,
    fecha_asignacion DATE,
    activo BOOLEAN,
    FOREIGN KEY (id_usuario_superior) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_usuario_subordinado) REFERENCES Usuario(id_usuario)
);

CREATE TABLE Log_Auditoria (
    idLogAuditoria INT PRIMARY KEY AUTO_INCREMENT,
    descripcion VARCHAR(150),
    accion VARCHAR(70),
    fecha_registro DATETIME,
    origenAccion VARCHAR(150),
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE Notificacion (
    id_notificacion INT PRIMARY KEY AUTO_INCREMENT,
    mensaje VARCHAR(150),
    tipo_notificacion VARCHAR(70),
    fecha_envio DATETIME,
    leido BOOLEAN,
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE Reserva (
    id_reserva INT PRIMARY KEY AUTO_INCREMENT,
    fecha_registro DATETIME,
    estado_reserva VARCHAR(15),
    cantidad_boletos INT,
    monto_total DOUBLE,
    fecha_ultima_modif DATETIME,
    canal_venta VARCHAR(50),
    monto_impuestos DOUBLE,
    codigo_bokun VARCHAR(80),
    id_usuario INT,
    id_cliente INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_cliente) REFERENCES Cliente(id_cliente)
);

CREATE TABLE Detalle_Reserva (
    id_detalle_reserva INT PRIMARY KEY AUTO_INCREMENT,
    id_reserva INT,
    id_servicio INT,
    cantidad INT,
    subtotal DOUBLE,
    FOREIGN KEY (id_reserva) REFERENCES Reserva(id_reserva),
    FOREIGN KEY (id_servicio) REFERENCES Servicio(id_servicio)
);

CREATE TABLE Reclamo (
    id_reclamo INT PRIMARY KEY AUTO_INCREMENT,
    fecha_reclamo DATETIME,
    descripcion VARCHAR(80),
    estado_reclamo VARCHAR(20),
    motivo_resolucion VARCHAR(70),
    fecha_resolucion DATE,
    id_usuario INT,
    id_reserva INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario),
    FOREIGN KEY (id_reserva) REFERENCES Reserva(id_reserva)
);

CREATE TABLE WebhookLog (
    id_log INT PRIMARY KEY AUTO_INCREMENT,
    bookingId VARCHAR(80),
    timestamp DATETIME,
    raw_data LONGTEXT, -- Se utiliza LONGTEXT para representar varchar2(max) en SQL estándar
    id_reserva INT NULL, -- Permite nulo según nota del DER
    FOREIGN KEY (id_reserva) REFERENCES Reserva(id_reserva)
);

CREATE TABLE Reporte_Ventas (
    id_reporte_ventas INT PRIMARY KEY AUTO_INCREMENT,
    fecha_generacion DATE,
    fecha_inicio_filtro DATE,
    fecha_fin_filtro DATE,
    cantidad_registros INT,
    monto_total DOUBLE,
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE Detalle_Reporte_Ventas (
    id_detalle_reporte_ventas INT PRIMARY KEY AUTO_INCREMENT,
    id_reporte_ventas INT,
    id_reservas INT,
    FOREIGN KEY (id_reporte_ventas) REFERENCES Reporte_Ventas(id_reporte_ventas),
    FOREIGN KEY (id_reservas) REFERENCES Reserva(id_reserva)
);

CREATE TABLE Reporte_Reclamo (
    id_reporte_reclamo INT PRIMARY KEY AUTO_INCREMENT,
    fecha_generacion DATE,
    fecha_inicio_filtro DATE,
    fecha_fin_filtro DATE,
    cantidad_reservas INT,
    cantidad_reclamos INT,
    porcentaje_incidencias DOUBLE,
    total_procede INT,
    total_no_procede INT,
    total_pendientes INT,
    id_usuario INT,
    FOREIGN KEY (id_usuario) REFERENCES Usuario(id_usuario)
);

CREATE TABLE Detalle_Reporte_Reclamo (
    id_detalle_reporte_reclamo INT PRIMARY KEY AUTO_INCREMENT,
    id_reporte_reclamo INT,
    id_reclamo INT,
    FOREIGN KEY (id_reporte_reclamo) REFERENCES Reporte_Reclamo(id_reporte_reclamo),
    FOREIGN KEY (id_reclamo) REFERENCES Reclamo(id_reclamo)
);