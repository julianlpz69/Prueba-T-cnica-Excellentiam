# Prueba-Tecnica-Excellentiam

Prueba Técnica en la que demostrara mis habilidades en para el puesto de desarrollador .Net
 
**Tiempo Gastado**: 6 horas

<br>

 
## 📁 Ejecución del proyecto

> **Requisitos previos**
>
> - Tener instalado **.NET SDK 9.0**
> - Tener acceso a una instancia de **SQL Server** (local o en contenedor Docker)

### 1) Configurar la cadena de conexión

Editar el archivo **`AcademyApp.Web/appsettings.json`** y reemplazar la cadena por la de tu servidor SQL Server:

<br>

### 2) Subir la base de datos

<br>

Opción A — Usar migraciones de EF Core
Desde la **raíz del proyecto** ejecutar:

```bash
dotnet ef database update -p AcademyApp.Infrastructure -s AcademyApp.Web
```
<br>

luego ejecutar en el proyecto web

```bash
dotnet run
```

<br>

## ✅ Conclucion

Gracias por la oportunidad de trabajar en este desafío técnico. Estoy feliz de responder cualquier pregunta o proporcionar explicaciones adicionales si es necesario.

**Julian Jose Lopez Arellano**
