# XcConnect

> XcConnect es una solución de CRM diseñada para micro pymes. Les ofrece la posibilidad de llevar un registro de su proceso de ventas y la gestión del relacionamiento con sus clientes.

# Características!

  - Creación de Vendedores
  - Creación de Clientes y Contactos
  - Registro de Productos o Servicios que ofrece la empresa
  - Creación de Oportunidades (Leads)
  - Creación de Cotizaciones
  - Registro de actividades operativas o recordatorios como llamadas, envio de correo electrónico o envío de documentación.
  - Reportes
  - Tablero de control con resumen de actividades próximas a vencer y gráficos.
  
# Requisitos de Instalación

Para la instalación de la aplicación se requiere:

* Si es on-premise Servidor IIS 7.0 o Superior, si es en Azure debe provisionar un app services (web app)
* Base de Datos SQL Server si es on-premise o SQL Azure. Recuerde que puede usar la versión gratis de [SQL Server Express](https://www.microsoft.com/en-us/sql-server/sql-server-editions-express)
* La solución puede editarse con el editor gratis de [Visual Studio Code](https://code.visualstudio.com) o con [Visual Studio Community 2015 Update 3 o Superior](https://www.visualstudio.com/vs/community)

# Instalación y Configuración

## Crear Base de Datos
La base de datos debe llamarse XcConnect, creela y ejecute el script XcConnectDB.sql. Debe tener en cuenta el modelo de autenticación configurado en el servidor para configurar correctamente la cadena de conexión en la aplicación, de lo contrario no tendrá acceso a la aplicación.

## Web Config

En el archivo web.config debe actualizar los siguientes settings:

### Correo electrónico
    <add key="SmtpUsrCredentials" value="" />
    <add key="SmtpPwdCredentials" value="" />
    <add key="SmtpUsrDisplayName" value="" />

      <smtp from="correo@dominio.com">
        <network host="smtp.servidorcorreo.com" port="puerto" />
      </smtp>

### Conexión a Base de Datos
Configure la conexión a la base de datos modificando los valores Server, Database, User y Password en la cadena de conexión.

    <add name="EEContext" connectionString="Data Source=[Server];Initial Catalog=[Database];User Id=[User];Password=[Password];" providerName="System.Data.SqlClient" />
    
### Inicializar Aplicación
Cambie el valor de la clave UpdateDataBase a true siguientes valores el web.config. Debe crear manualmente la primera compañía en la tabla Empresas de la base de datos y Actualizar la clave SuperUsrEmpresaId con el ID asignado. 

    <add key="UpdateDataBase" value="false" />
    <add key="SuperUsrEmpresaId" value="1" />
    
Antes de iniciar la aplicación, debe asegurarse de modifcar las constantes InitialUserName y InitialUserPwd en la clase startup.containers.cs

    private const string InitialUserName = "";
    private const string InitialUserPwd = "";

    user.FirstName = "";
    user.LastName = "";
    user.Email = "";


Una vez modificado estos valores, ejecute la aplicación para que se configure la seguridad para iniciar la aplicación con un usuario tipo Super Administrador.
