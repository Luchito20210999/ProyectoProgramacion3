# ProyectoProgramacion3 - Parte C#

Este directorio contiene la parte desarrollada en **C#** del proyecto "ProyectoProgramacion3".

## Contexto Actual y Evolución del Proyecto

Inicialmente, el proyecto en C# se estructuró siguiendo una **Arquitectura en Capas (N-Tier)**, lo cual explica la existencia actual de múltiples subproyectos:
- **ProyectoProgramacion3Model**
- **ProyectoProgramacion3DBManager**
- **ProyectoProgramacion3Persistencia**
- **ProyectoProgramacion3Negocio**
- **ProyectoProgramacion3Web**

### ⚠️ Próximos Cambios Importantes (Migración de Arquitectura)

La visión actual y definitiva del proyecto es **utilizar C# de manera exclusiva como Frontend (Capa de Presentación)**. 

Por lo tanto, los proyectos de **Persistencia, DBManager, Model y Negocio serán eliminados próximamente**. La lógica de negocio, el acceso a datos y el modelo central se trasladarán y gestionarán completamente en el proyecto **JAVA**. 

Se establecerá una conexión directa entre los dos proyectos, donde:
1. **JAVA** actuará como el núcleo del sistema (Backend, API, Lógica y Persistencia).
2. **C# (ProyectoProgramacion3Web)** actuará únicamente como el cliente (Frontend) que consumirá los servicios expuestos por la parte de Java.

## Futuras Integraciones (Parte JAVA)

A medida que el backend se centralice en Java, se tiene planificado incluir la integración con **Bókun** dentro del apartado del proyecto en JAVA. Esto permitirá expandir las capacidades del sistema aprovechando las herramientas de gestión de reservas o tours que ofrece Bókun.

## Consideraciones para el Desarrollo Actual

- Hasta que se concrete la eliminación de las capas de negocio y persistencia en C#, el enfoque de desarrollo debe priorizar la preparación del proyecto **Web** para su futura independencia.
- Para abrir el proyecto, utiliza el archivo de solución `ProyectoProgramacion3.slnx` con un IDE compatible como Visual Studio o Rider.
