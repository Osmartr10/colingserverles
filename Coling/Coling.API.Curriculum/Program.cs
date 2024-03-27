
using Coling.API.Curriculum.Contratos.Repositorio;
using Coling.API.Curriculum.Implementacion.Repositorio;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenApi.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Coling.Utilitarios.Middlewares;
using System.Reflection;
using Coling.Utilitarios.Attributes;
using System.Runtime.CompilerServices;

var ensamblado = Assembly.GetExecutingAssembly();
var tipoAtributo = typeof(ColingAuthorizeAttribute);

var host = new HostBuilder()    
    .ConfigureServices(services =>
    {
        //services.AddAutoMapper(ensamblado);
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddScoped<IInstitucionRepositorio, InstitucionRepositorio>();
        services.AddSingleton<JwtMiddleware>();
        services.AddSingleton<AutorizacionRolMiddleware>();
    }).ConfigureFunctionsWebApplication(x =>
    {
        x.UseMiddleware<JwtMiddleware>();
        x.UseMiddleware<AutorizacionRolMiddleware>();
    })
    .Build();
var lista = ListarFuncionesAttributes(ensamblado, tipoAtributo);
host.Run();

List<string> ListarFuncionesAttributes(Assembly ensambladox, Type tipox)
{
    List<string> lista = new List<string>();
    Type tipoFuncion = typeof(FunctionAttribute);
    var metodos = ObtnerMetodosConAtributos(ensambladox, tipox);

    return lista;
}

MethodInfo[] ObtnerMetodosConAtributos(Type type, Type tipoAtributo)
{
    return type.GetMethods().Where(m=> m.GetCustomAttributes(tipoAtributo, inherit:true).Any()).ToArray();
}