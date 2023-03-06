一些简单的Demo，希望能够帮助更好的构建asp.net项目

## JWT目录
### **JWT.DemoApi项目**
是提供接口服务的WebApi，客户端（Web、App）会通过该WebApi获得数据，其不提供登陆服务，登陆服务由JWT.Server提供，访问受限接口需要JWT.Server所发放的Access_Token。
### **JWT.Server项目**
是提供登陆服务，客户端通过访问 `api/Auth/Authenticate` 接口使用用户名与密码验证其用户合法性，合法的就发放JWT类型Access_Token，使用该Access_Token就可以访问JWT.DemoApi的受限接口。


