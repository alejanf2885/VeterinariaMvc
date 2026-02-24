# 🐾 VetCore DB: Sistema de Gestión Veterinaria

Este repositorio contiene el diseño estructural de la base de datos para un sistema integral de gestión clínica veterinaria. La arquitectura está optimizada para entornos **MVC (Modelo-Vista-Controlador)** y utiliza **SQL Server** como motor de persistencia.



## 🚀 Características Principales

* **Identidad Centralizada:** Sistema multi-rol (Administrador, Veterinario, Cliente) vinculado a una tabla de usuarios única.
* **Fichas Médicas Dinámicas (EAV):** Motor de plantillas que permite crear exámenes personalizados sin modificar el esquema de la base de datos.
* **Gestión Multimedia:** Repositorio normalizado para radiografías, analíticas y fotografías clínicas con trazabilidad total.
* **Seguimiento Colaborativo:** Espacio interactivo para que veterinarios y dueños registren la evolución de los tratamientos.
* **Arquitectura Multi-Clínica:** Relación flexible para que las mascotas puedan ser atendidas en distintas sedes.

---

## 🏗️ Estructura de Módulos

### 1. Seguridad y Acceso
Gestiona quién entra al sistema y qué puede hacer.
- `ROL`: Catálogo de permisos.
- `USUARIO`: Tabla maestra de credenciales.
- `VETERINARIO` / `CLIENTE`: Extensiones de perfil con datos específicos.
- `CLINICA`: Registro de sedes físicas.

### 2. Gestión de Pacientes
- `MASCOTA`: Datos biográficos y estado vital del animal.
- `MASCOTA_CLINICA`: Control de alta de mascotas en diferentes centros veterinarios.

### 3. Actividad Clínica y Archivos
- `CONSULTA`: Registro de citas (motivo, fecha y estado).
- `FICHA_CONSULTA`: El informe médico detallado resultante de la visita.
- `TIPO_ARCHIVO`: Diccionario de categorías (Ej: Radiografía, Ecografía).
- `FICHA_ARCHIVO`: Almacenamiento de URLs de archivos multimedia con registro de autoría.



### 4. Motor de Plantillas (EAV)
Permite la creación de formularios dinámicos para diferentes especialidades médicas.
- `PLANTILLA`: El "esqueleto" del informe (Ej: "Cirugía de Tejidos Blandos").
- `PLANTILLA_SECCION` / `PLANTILLA_CAMPO`: Estructura y orden de las preguntas.
- `FICHA_VALOR`: Almacena los datos específicos (textos, números, fechas o booleanos).

### 5. Tratamientos y Evolución
- `TRATAMIENTO`: Plan de acción médico asignado a una mascota.
- `SEGUIMIENTO_TRATAMIENTO`: Diario de recuperación con comentarios fechados automáticamente.

---

## 📊 Diccionario de Datos (Principales Tablas)

| Tabla | Propósito | Relación Clave |
| :--- | :--- | :--- |
| **USUARIO** | Autenticación y datos personales. | `ID_ROL` |
| **MASCOTA** | Registro del paciente. | `ID_CLIENTE` |
| **CONSULTA** | Agendamiento y registro de visitas. | `ID_MASCOTA`, `ID_VETERINARIO` |
| **FICHA_VALOR** | Datos técnicos de la ficha médica. | `ID_FICHA`, `ID_CAMPO` |
| **FICHA_ARCHIVO** | Almacenamiento de pruebas diagnósticas. | `ID_FICHA`, `ID_TIPO` |

---

## 🛠️ Notas Técnicas para Desarrolladores

1.  **Integridad:** Todas las tablas cuentan con llaves primarias `IDENTITY(1,1)` y llaves foráneas activas.
2.  **Fechas Automáticas:** Se utiliza `CURRENT_TIMESTAMP` en campos de auditoría (`FECHA_CREACION`, `FECHA_SUBIDA`, `FECHA` en seguimientos).
3.  **Trazabilidad:** La tabla `FICHA_ARCHIVO` requiere un `ID_USUARIO` para registrar legalmente quién incorporó cada prueba al expediente.
4.  **MVC:** Se recomienda el uso de **Procedimientos Almacenados** para inserciones complejas (ej: crear usuario y cliente en una sola transacción).

---

## 📈 Roadmap de Desarrollo (Fases)

1.  **Fase 1:** Seguridad, usuarios y perfiles.
2.  **Fase 2:** Gestión de mascotas y dueños.
3.  **Fase 3:** Consultas, plantillas dinámicas y carga de archivos.
4.  **Fase 4:** Tratamientos, seguimientos y reportes estadísticos.

---
*Este diseño fue generado para ser escalable, permitiendo la futura integración de módulos de mensajería (chats) o facturación sin afectar la estructura base.*