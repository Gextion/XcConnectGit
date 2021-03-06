USE [XcConnectBD]
GO
/****** Object:  Table [dbo].[Actividades]    Script Date: 12/03/2018 6:52:43 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actividades](
	[ActividadID] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] [date] NOT NULL,
	[Descripcion] [nvarchar](100) NOT NULL,
	[TipoActividadID] [int] NOT NULL,
	[EmpresaID] [int] NOT NULL,
	[FechaEntrega] [date] NOT NULL,
	[Notas] [nvarchar](max) NULL,
	[UserID] [nvarchar](128) NULL,
 CONSTRAINT [PK_Actividades] PRIMARY KEY CLUSTERED 
(
	[ActividadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActividadesArchivos]    Script Date: 12/03/2018 6:52:43 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActividadesArchivos](
	[ActividadArchivoID] [int] IDENTITY(1,1) NOT NULL,
	[ActividadID] [int] NOT NULL,
	[ArchivoUrl] [nvarchar](500) NOT NULL,
	[LocalUrl] [nvarchar](500) NULL,
	[ArchivoNombre] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_ActividadesArchivos] PRIMARY KEY CLUSTERED 
(
	[ActividadArchivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Años]    Script Date: 12/03/2018 6:52:44 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Años](
	[AñoID] [int] NOT NULL,
	[Año] [int] NOT NULL,
 CONSTRAINT [PK_Años] PRIMARY KEY CLUSTERED 
(
	[AñoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationRoleGroups]    Script Date: 12/03/2018 6:52:44 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationRoleGroups](
	[RoleId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationRoleGroups] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ApplicationUserGroups]    Script Date: 12/03/2018 6:52:44 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationUserGroups](
	[UserId] [nvarchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.ApplicationUserGroups] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[GroupId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetRoles]    Script Date: 12/03/2018 6:52:45 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetRoles](
	[Id] [nvarchar](128) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetRoles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserLogins]    Script Date: 12/03/2018 6:52:45 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserLogins](
	[UserId] [nvarchar](128) NOT NULL,
	[LoginProvider] [nvarchar](128) NOT NULL,
	[ProviderKey] [nvarchar](128) NOT NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserLogins] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[LoginProvider] ASC,
	[ProviderKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUserRoles]    Script Date: 12/03/2018 6:52:45 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUserRoles](
	[UserId] [nvarchar](128) NOT NULL,
	[RoleId] [nvarchar](128) NOT NULL,
	[IdentityRole_Id] [nvarchar](128) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.AspNetUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AspNetUsers]    Script Date: 12/03/2018 6:52:45 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AspNetUsers](
	[Id] [nvarchar](128) NOT NULL,
	[Email] [nvarchar](max) NULL,
	[EmailConfirmed] [bit] NOT NULL,
	[PasswordHash] [nvarchar](max) NULL,
	[SecurityStamp] [nvarchar](max) NULL,
	[PhoneNumber] [nvarchar](max) NULL,
	[PhoneNumberConfirmed] [bit] NOT NULL,
	[TwoFactorEnabled] [bit] NOT NULL,
	[LockoutEndDateUtc] [datetime] NULL,
	[LockoutEnabled] [bit] NOT NULL,
	[AccessFailedCount] [int] NOT NULL,
	[UserName] [nvarchar](max) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[EmpresaID] [int] NULL,
	[Discriminator] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_dbo.AspNetUsers] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditTemp]    Script Date: 12/03/2018 6:52:46 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditTemp](
	[IDRow] [int] NOT NULL,
	[EmpresaNombre] [nvarchar](80) NULL,
	[EmpresaNIT] [bigint] NULL,
	[FechaIngreso] [datetime] NULL,
 CONSTRAINT [PK_AuditTemp] PRIMARY KEY CLUSTERED 
(
	[IDRow] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuthenticationAudit]    Script Date: 12/03/2018 6:52:46 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuthenticationAudit](
	[AuditID] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](128) NOT NULL,
	[LoginIP] [nvarchar](50) NULL,
	[LoginBrowser] [nvarchar](150) NULL,
	[LoginBrowserVersion] [nvarchar](150) NULL,
	[LoginPlatform] [nvarchar](150) NULL,
	[LoginDate] [datetime] NOT NULL,
 CONSTRAINT [PK_AuthenticationAudit] PRIMARY KEY CLUSTERED 
(
	[AuditID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ciudades]    Script Date: 12/03/2018 6:52:46 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ciudades](
	[CiudadID] [int] IDENTITY(1,1) NOT NULL,
	[Ciudad] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Ciudades] PRIMARY KEY CLUSTERED 
(
	[CiudadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clientes]    Script Date: 12/03/2018 6:52:46 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clientes](
	[ClienteID] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Nit] [bigint] NOT NULL,
	[RazonSocial] [nvarchar](80) NULL,
	[RepresentanteLegal] [nvarchar](80) NULL,
	[EmpresaID] [int] NOT NULL,
	[VendedorID] [int] NOT NULL,
	[SectorEconomicoID] [int] NOT NULL,
	[CiudadID] [int] NOT NULL,
	[Direccion] [nvarchar](120) NULL,
	[Telefono] [bigint] NULL,
	[Celular] [bigint] NULL,
	[Email] [nvarchar](100) NULL,
	[SitioWeb] [nvarchar](300) NULL,
 CONSTRAINT [PK_dbo.Clientes] PRIMARY KEY CLUSTERED 
(
	[ClienteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Contactos]    Script Date: 12/03/2018 6:52:47 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Contactos](
	[ContactoID] [int] IDENTITY(1,1) NOT NULL,
	[ClienteID] [int] NOT NULL,
	[Nombre] [nvarchar](60) NOT NULL,
	[Cargo] [nvarchar](50) NOT NULL,
	[Telefono] [bigint] NULL,
	[Celular] [bigint] NULL,
	[Email] [nvarchar](100) NULL,
 CONSTRAINT [PK_Contactos] PRIMARY KEY CLUSTERED 
(
	[ContactoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cotizacion]    Script Date: 12/03/2018 6:52:47 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cotizacion](
	[CotizacionID] [int] IDENTITY(1,1) NOT NULL,
	[NumberID] [nvarchar](10) NULL,
	[EmpresaID] [int] NOT NULL,
	[Fecha] [datetime] NULL,
	[VendedorID] [int] NOT NULL,
	[ClienteID] [int] NOT NULL,
	[Valor] [decimal](18, 2) NULL,
	[OportunidadID] [int] NULL,
	[ActividadID] [int] NULL,
 CONSTRAINT [PK_Cotizacion] PRIMARY KEY CLUSTERED 
(
	[CotizacionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CotizacionConsecutivo]    Script Date: 12/03/2018 6:52:47 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CotizacionConsecutivo](
	[ConsecutivoID] [int] IDENTITY(1,1) NOT NULL,
	[EmpresaID] [int] NOT NULL,
	[ValorConsecutivo] [int] NOT NULL,
 CONSTRAINT [PK_CotizacionConsecutivo] PRIMARY KEY CLUSTERED 
(
	[ConsecutivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CotizacionDetalle]    Script Date: 12/03/2018 6:52:47 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CotizacionDetalle](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[CotizacionID] [int] NOT NULL,
	[ProductoID] [int] NOT NULL,
	[Cantidad] [decimal](18, 2) NOT NULL,
	[ValUnitario] [decimal](18, 2) NOT NULL,
	[ValTotal] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_CotizacionDetalle] PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Empresas]    Script Date: 12/03/2018 6:52:48 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Empresas](
	[EmpresaID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](4) NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[RazonSocial] [nvarchar](80) NOT NULL,
	[Nit] [bigint] NOT NULL,
	[CiudadID] [int] NOT NULL,
	[Direccion] [nvarchar](120) NOT NULL,
	[Telefono] [bigint] NOT NULL,
	[Celular] [bigint] NULL,
	[Email] [nvarchar](100) NULL,
	[SitioWeb] [nvarchar](300) NULL,
	[LogoURL] [nvarchar](300) NULL,
	[LineaBase] [int] NOT NULL,
 CONSTRAINT [PK_dbo.Empresas] PRIMARY KEY CLUSTERED 
(
	[EmpresaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EstadosOportunidad]    Script Date: 12/03/2018 6:52:48 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EstadosOportunidad](
	[EstadoOportunidadID] [int] NOT NULL,
	[EstadoOportunidad] [nvarchar](50) NOT NULL,
	[RGBA_Background] [nvarchar](50) NULL,
 CONSTRAINT [PK_EstadosOportunidad] PRIMARY KEY CLUSTERED 
(
	[EstadoOportunidadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 12/03/2018 6:52:48 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Groups](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_dbo.Groups] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IdentityUserClaims]    Script Date: 12/03/2018 6:52:48 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IdentityUserClaims](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [nvarchar](max) NULL,
	[ClaimType] [nvarchar](max) NULL,
	[ClaimValue] [nvarchar](max) NULL,
	[IdentityUser_Id] [nvarchar](128) NULL,
 CONSTRAINT [PK_dbo.IdentityUserClaims] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Meses]    Script Date: 12/03/2018 6:52:49 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meses](
	[MesID] [int] NOT NULL,
	[Mes] [nvarchar](15) NOT NULL,
	[RGBA_Background] [nvarchar](50) NULL,
	[RGBA_Border] [nvarchar](50) NULL,
 CONSTRAINT [PK_Meses] PRIMARY KEY CLUSTERED 
(
	[MesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Oportunidades]    Script Date: 12/03/2018 6:52:49 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Oportunidades](
	[OportunidadID] [int] IDENTITY(1,1) NOT NULL,
	[NombreOportunidad] [nvarchar](80) NOT NULL,
	[Descripcion] [nvarchar](400) NOT NULL,
	[Estado] [int] NOT NULL,
	[ClienteID] [int] NOT NULL,
	[SolicitadaPor] [nvarchar](60) NOT NULL,
	[VendedorID] [int] NOT NULL,
	[FechaSolicitud] [date] NOT NULL,
	[FechaEntrega] [date] NULL,
	[FechaCierre] [date] NULL,
	[Valor] [int] NULL,
	[UserID] [nchar](128) NULL,
 CONSTRAINT [PK_Oportunidades] PRIMARY KEY CLUSTERED 
(
	[OportunidadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OportunidadesArchivos]    Script Date: 12/03/2018 6:52:49 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OportunidadesArchivos](
	[OportunidadArchivoID] [int] IDENTITY(1,1) NOT NULL,
	[OportunidadID] [int] NOT NULL,
	[ArchivoUrl] [nvarchar](500) NOT NULL,
	[LocalUrl] [nvarchar](500) NULL,
	[ArchivoNombre] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_OportunidadesArchivos] PRIMARY KEY CLUSTERED 
(
	[OportunidadArchivoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Peticiones]    Script Date: 12/03/2018 6:52:49 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Peticiones](
	[PeticionID] [int] IDENTITY(1,1) NOT NULL,
	[FechaRegistro] [date] NOT NULL,
	[Titulo] [nvarchar](60) NOT NULL,
	[TipoPeticion] [int] NOT NULL,
	[Descripcion] [nvarchar](600) NOT NULL,
	[FechaSolucion] [date] NULL,
	[Solucion] [nvarchar](600) NULL,
	[ResueltaPor] [nvarchar](60) NULL,
	[UserID] [nvarchar](128) NULL,
	[EmpresaID] [int] NOT NULL,
 CONSTRAINT [PK_Peticiones] PRIMARY KEY CLUSTERED 
(
	[PeticionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Productos]    Script Date: 12/03/2018 6:52:50 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Productos](
	[ProductoID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](20) NOT NULL,
	[NombreProducto] [nvarchar](120) NOT NULL,
	[Especificaciones] [nvarchar](max) NULL,
	[Valor] [decimal](18, 2) NOT NULL,
	[EmpresaID] [int] NOT NULL,
 CONSTRAINT [PK_Productos] PRIMARY KEY CLUSTERED 
(
	[ProductoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SectoresEconomicos]    Script Date: 12/03/2018 6:52:50 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SectoresEconomicos](
	[SectorEconomicoID] [int] IDENTITY(1,1) NOT NULL,
	[SectorEconomico] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.SectoresEconomicos] PRIMARY KEY CLUSTERED 
(
	[SectorEconomicoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TiposActividades]    Script Date: 12/03/2018 6:52:50 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TiposActividades](
	[TipoActividadID] [int] NOT NULL,
	[NombreTipoActividad] [nvarchar](50) NOT NULL,
	[Icono] [nvarchar](30) NULL,
 CONSTRAINT [PK_TiposActividades] PRIMARY KEY CLUSTERED 
(
	[TipoActividadID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendedores]    Script Date: 12/03/2018 6:52:51 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendedores](
	[VendedorID] [int] IDENTITY(1,1) NOT NULL,
	[Codigo] [nvarchar](8) NOT NULL,
	[NombreVendedor] [nvarchar](50) NOT NULL,
	[Celular] [bigint] NOT NULL,
	[Email] [nvarchar](60) NOT NULL,
	[EmpresaID] [int] NOT NULL,
 CONSTRAINT [PK_Vendedores] PRIMARY KEY CLUSTERED 
(
	[VendedorID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[CotizacionConsecutivo] ADD  CONSTRAINT [DF_CotizacionConsecutivo_ValorConsecutivo]  DEFAULT ((0)) FOR [ValorConsecutivo]
GO
ALTER TABLE [dbo].[Vendedores] ADD  CONSTRAINT [DF_Vendedores_Código]  DEFAULT ('') FOR [Codigo]
GO
ALTER TABLE [dbo].[Actividades]  WITH CHECK ADD  CONSTRAINT [FK_Actividades_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Actividades] CHECK CONSTRAINT [FK_Actividades_Empresas]
GO
ALTER TABLE [dbo].[Actividades]  WITH CHECK ADD  CONSTRAINT [FK_Actividades_TiposActividades] FOREIGN KEY([TipoActividadID])
REFERENCES [dbo].[TiposActividades] ([TipoActividadID])
GO
ALTER TABLE [dbo].[Actividades] CHECK CONSTRAINT [FK_Actividades_TiposActividades]
GO
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId] FOREIGN KEY([RoleId])
REFERENCES [dbo].[AspNetRoles] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.AspNetRoles_RoleId]
GO
ALTER TABLE [dbo].[ApplicationRoleGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationRoleGroups] CHECK CONSTRAINT [FK_dbo.ApplicationRoleGroups_dbo.Groups_GroupId]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.AspNetUsers_UserId]
GO
ALTER TABLE [dbo].[ApplicationUserGroups]  WITH CHECK ADD  CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.Groups_GroupId] FOREIGN KEY([GroupId])
REFERENCES [dbo].[Groups] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationUserGroups] CHECK CONSTRAINT [FK_dbo.ApplicationUserGroups_dbo.Groups_GroupId]
GO
ALTER TABLE [dbo].[AspNetUserLogins]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserLogins] CHECK CONSTRAINT [FK_dbo.AspNetUserLogins_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id] FOREIGN KEY([IdentityRole_Id])
REFERENCES [dbo].[AspNetRoles] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetRoles_IdentityRole_Id]
GO
ALTER TABLE [dbo].[AspNetUserRoles]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[AspNetUserRoles] CHECK CONSTRAINT [FK_dbo.AspNetUserRoles_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[AspNetUsers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.AspNetUsers_dbo.Empresas_EmpresaID] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AspNetUsers] CHECK CONSTRAINT [FK_dbo.AspNetUsers_dbo.Empresas_EmpresaID]
GO
ALTER TABLE [dbo].[AuthenticationAudit]  WITH CHECK ADD  CONSTRAINT [FK_AuthenticationAudit_AspNetUsers] FOREIGN KEY([UserId])
REFERENCES [dbo].[AspNetUsers] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthenticationAudit] CHECK CONSTRAINT [FK_AuthenticationAudit_AspNetUsers]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Ciudades] FOREIGN KEY([CiudadID])
REFERENCES [dbo].[Ciudades] ([CiudadID])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Ciudades]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Empresas]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_Clientes_Vendedores] FOREIGN KEY([VendedorID])
REFERENCES [dbo].[Vendedores] ([VendedorID])
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_Clientes_Vendedores]
GO
ALTER TABLE [dbo].[Clientes]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Clientes_dbo.SectoresEconomicos_SectorEconomicoID] FOREIGN KEY([SectorEconomicoID])
REFERENCES [dbo].[SectoresEconomicos] ([SectorEconomicoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Clientes] CHECK CONSTRAINT [FK_dbo.Clientes_dbo.SectoresEconomicos_SectorEconomicoID]
GO
ALTER TABLE [dbo].[Contactos]  WITH CHECK ADD  CONSTRAINT [FK_Contactos_Clientes] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[Contactos] CHECK CONSTRAINT [FK_Contactos_Clientes]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Actividades] FOREIGN KEY([ActividadID])
REFERENCES [dbo].[Actividades] ([ActividadID])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Actividades]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Clientes] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Clientes]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Empresas]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Oportunidades] FOREIGN KEY([OportunidadID])
REFERENCES [dbo].[Oportunidades] ([OportunidadID])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Oportunidades]
GO
ALTER TABLE [dbo].[Cotizacion]  WITH CHECK ADD  CONSTRAINT [FK_Cotizacion_Vendedores] FOREIGN KEY([VendedorID])
REFERENCES [dbo].[Vendedores] ([VendedorID])
GO
ALTER TABLE [dbo].[Cotizacion] CHECK CONSTRAINT [FK_Cotizacion_Vendedores]
GO
ALTER TABLE [dbo].[CotizacionConsecutivo]  WITH CHECK ADD  CONSTRAINT [FK_CotizacionConsecutivo_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[CotizacionConsecutivo] CHECK CONSTRAINT [FK_CotizacionConsecutivo_Empresas]
GO
ALTER TABLE [dbo].[CotizacionDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CotizacionDetalle_Cotizacion] FOREIGN KEY([CotizacionID])
REFERENCES [dbo].[Cotizacion] ([CotizacionID])
GO
ALTER TABLE [dbo].[CotizacionDetalle] CHECK CONSTRAINT [FK_CotizacionDetalle_Cotizacion]
GO
ALTER TABLE [dbo].[CotizacionDetalle]  WITH CHECK ADD  CONSTRAINT [FK_CotizacionDetalle_Productos] FOREIGN KEY([ProductoID])
REFERENCES [dbo].[Productos] ([ProductoID])
GO
ALTER TABLE [dbo].[CotizacionDetalle] CHECK CONSTRAINT [FK_CotizacionDetalle_Productos]
GO
ALTER TABLE [dbo].[Empresas]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Empresas_dbo.Ciudades_CiudadID] FOREIGN KEY([CiudadID])
REFERENCES [dbo].[Ciudades] ([CiudadID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Empresas] CHECK CONSTRAINT [FK_dbo.Empresas_dbo.Ciudades_CiudadID]
GO
ALTER TABLE [dbo].[Empresas]  WITH CHECK ADD  CONSTRAINT [FK_Empresas_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Empresas] CHECK CONSTRAINT [FK_Empresas_Empresas]
GO
ALTER TABLE [dbo].[Empresas]  WITH CHECK ADD  CONSTRAINT [FK_Empresas_Empresas1] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Empresas] CHECK CONSTRAINT [FK_Empresas_Empresas1]
GO
ALTER TABLE [dbo].[IdentityUserClaims]  WITH CHECK ADD  CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id] FOREIGN KEY([IdentityUser_Id])
REFERENCES [dbo].[AspNetUsers] ([Id])
GO
ALTER TABLE [dbo].[IdentityUserClaims] CHECK CONSTRAINT [FK_dbo.IdentityUserClaims_dbo.AspNetUsers_IdentityUser_Id]
GO
ALTER TABLE [dbo].[Oportunidades]  WITH CHECK ADD  CONSTRAINT [FK_Oportunidades_Clientes] FOREIGN KEY([ClienteID])
REFERENCES [dbo].[Clientes] ([ClienteID])
GO
ALTER TABLE [dbo].[Oportunidades] CHECK CONSTRAINT [FK_Oportunidades_Clientes]
GO
ALTER TABLE [dbo].[Oportunidades]  WITH CHECK ADD  CONSTRAINT [FK_Oportunidades_Vendedores] FOREIGN KEY([VendedorID])
REFERENCES [dbo].[Vendedores] ([VendedorID])
GO
ALTER TABLE [dbo].[Oportunidades] CHECK CONSTRAINT [FK_Oportunidades_Vendedores]
GO
ALTER TABLE [dbo].[OportunidadesArchivos]  WITH CHECK ADD  CONSTRAINT [FK_OportunidadesArchivos_Oportunidades] FOREIGN KEY([OportunidadID])
REFERENCES [dbo].[Oportunidades] ([OportunidadID])
GO
ALTER TABLE [dbo].[OportunidadesArchivos] CHECK CONSTRAINT [FK_OportunidadesArchivos_Oportunidades]
GO
ALTER TABLE [dbo].[Peticiones]  WITH CHECK ADD  CONSTRAINT [FK_Peticiones_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Peticiones] CHECK CONSTRAINT [FK_Peticiones_Empresas]
GO
ALTER TABLE [dbo].[Productos]  WITH CHECK ADD  CONSTRAINT [FK_Productos_Productos] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Productos] CHECK CONSTRAINT [FK_Productos_Productos]
GO
ALTER TABLE [dbo].[Vendedores]  WITH CHECK ADD  CONSTRAINT [FK_Vendedores_Empresas] FOREIGN KEY([EmpresaID])
REFERENCES [dbo].[Empresas] ([EmpresaID])
GO
ALTER TABLE [dbo].[Vendedores] CHECK CONSTRAINT [FK_Vendedores_Empresas]
GO
/****** Object:  StoredProcedure [dbo].[spDashboard_Actividades]    Script Date: 12/03/2018 6:52:51 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboard_Actividades]

	@EmpresaID int,
	@TipoActividadID int,
	@Año int,
	@Mes int,
	@UsuarioID nvarchar(128)

AS

SELECT      TiposActividades.NombreTipoActividad AS TipoActividad, 
			YEAR(Actividades.FechaRegistro) AS Año, 
			MONTH(Actividades.FechaRegistro) AS Mes, 
			Meses.Mes AS NombreMes,
			Meses.RGBA_Background,
			Meses.RGBA_Border,
			COUNT(Actividades.ActividadID) AS Cantidad
FROM        Actividades 
			INNER JOIN TiposActividades ON Actividades.TipoActividadID = TiposActividades.TipoActividadID 
			INNER JOIN AspNetUsers ON Actividades.UserID = AspNetUsers.Id
			INNER JOIN Meses ON MONTH(Actividades.FechaRegistro) = Meses.MesID
WHERE		Actividades.EmpresaID = @EmpresaID
			AND (@TipoActividadID = 0 OR Actividades.TipoActividadID = @TipoActividadID)
			AND (@Año = 0 OR YEAR(Actividades.FechaRegistro) = @Año)
			AND (@Mes = 0  OR MONTH(Actividades.FechaRegistro) = @Mes) 
			AND (@UsuarioID = '' OR Actividades.UserID = @UsuarioID)
GROUP BY YEAR(Actividades.FechaRegistro), MONTH(Actividades.FechaRegistro), Meses.Mes, Meses.RGBA_Background, RGBA_Border, TiposActividades.NombreTipoActividad
ORDER BY YEAR(Actividades.FechaRegistro), MONTH(Actividades.FechaRegistro), TiposActividades.NombreTipoActividad



GO
/****** Object:  StoredProcedure [dbo].[spDashboard_Oportunidades]    Script Date: 12/03/2018 6:52:51 a.m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spDashboard_Oportunidades]

--DECLARE 

	@EmpresaID int,
	@Año int,
	@Mes int,
	@UsuarioID nvarchar(128)

AS

--SET @TipoActividadID = 1

SELECT      EstadosOportunidad.EstadoOportunidad, 
			EstadosOportunidad.RGBA_Background,
			COUNT(Oportunidades.OportunidadID) AS Cantidad
FROM        Oportunidades 
			INNER JOIN EstadosOportunidad ON Oportunidades.Estado = EstadosOportunidad.EstadoOportunidadID
			INNER JOIN Vendedores ON Oportunidades.VendedorID = Vendedores.VendedorID
			INNER JOIN AspNetUsers ON Oportunidades.UserID = AspNetUsers.Id
WHERE		Vendedores.EmpresaID = @EmpresaID
			AND (@Año = 0 OR YEAR(Oportunidades.FechaSolicitud) = @Año)
			AND (@Mes = 0  OR MONTH(Oportunidades.FechaSolicitud) = @Mes) 
			AND (@UsuarioID = '' OR Oportunidades.UserID = @UsuarioID)
GROUP BY EstadosOportunidad.EstadoOportunidad, EstadosOportunidad.RGBA_Background
GO
