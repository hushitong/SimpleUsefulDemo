# 如何使用swagger

## 简单使用


框架.net3.1默认没有swagger，需要自己弄，以下简单写步骤

1. nuget安装Swashbuckle.AspNetCore

   ```powershell
   Install-Package Swashbuckle.AspNetCore
   ```

2. 修改Startup.cs文件

   1. 引入命名空间：

      ```c#
      using Swashbuckle.AspNetCore.Swagger;
      using Microsoft.OpenApi.Models;
      ```

   2. 将 Swagger 生成器添加到 `Startup.ConfigureServices` 方法中的服务集合中

      ```c#
      //注册Swagger生成器，定义一个和多个Swagger 文档
      services.AddSwaggerGen(c =>
      {
           c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
      });
      ```

   3. 在 `Startup.Configure` 方法中，启用中间件为生成的 JSON 文档和 Swagger UI 提供服务

      ```c#
      //启用中间件服务生成Swagger作为JSON终结点
      app.UseSwagger();
      //启用中间件服务对swagger-ui，指定Swagger JSON终结点
      app.UseSwaggerUI(c =>
      {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });
      ```

   4. 此时启动项目就可通过`http://localhost:<port>/swagger`地址访问Swagger UI浏览API文档。

      也可通过`http://localhost:<port>/swagger/v1/swagger.json`地址访问生成的描述终结点的json文档。

   5. 如果要想通过`http://localhost:<port>/`就访问Swagger UI，那可以把前面步骤3修改的一个地方，把`RoutePrefix` 属性设置为空字符串。

      ```C#
      app.UseSwaggerUI(c =>
      {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
          c.RoutePrefix = string.Empty;
      });
      ```

   更加具体的可以自己找其他人的教程，swagger官网为：https://swagger.io/