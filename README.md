# AppSettingsManager 建立設定檔管理員
![https://opensource.com/sites/default/files/styles/image-full-size/public/lead-images/innovation_lightbulb_gears_devops_ansible.png?itok=TSbmp3_M](https://opensource.com/sites/default/files/styles/image-full-size/public/lead-images/innovation_lightbulb_gears_devops_ansible.png?itok=TSbmp3_M)

## 前言 🚁


在ASP.NET Core 以上的版本，要取得 appsettings.json 的設定資料，需以注入的方式取得，而常見的做法如下(以WEB API為例):

```csharp
public class AppSettingsDemoController : ControllerBase
{
    private readonly IConfiguration _configuration;
    public AppSettingsDemoController(IConfiguration configuration)
    {
	//DI注入，取得 configuration 實體
	_configuration = configuration;
    }

    [HttpGet("GetPasswordNotByExtension")]
    public ActionResult<string> GetPasswordNotByExtension()
    {
	//取得密碼資訊
        var myPassword = _configuration.GetValue<string>("MySettings:Password");

        return myPassword;
    }
}
```

從取得密碼資訊這段code可得知，若**"MySettings:Password"**名稱有變更，需針對所有有引用該設定字串的檔案進行修改，將造成管理上的困難。

接下來，會介紹如何建立設定檔管理員，讓開發者易於管理且更直覺的使用設定檔資訊。

## 建立設定檔管理員


**步驟一**

設定appsettings.json

```json
"MySettings": {
    "Account": "Alice_Account_123",
    "Password":  "Alice_Password_456"
}
```

**步驟二**

建立設定檔資訊model-MySettings

```csharp
public class MySettings
{
    public string Account { get; set; }
    public string Password { get; set; }
}
```

**步驟三**

建立 IServiceCollection 的擴充方法，在擴充方法裡使用Configuration的Bind()方法，將設定檔資訊與MySettings Instance 綁定，並設定單例注入MySettings Instance

```csharp
public static TSettingsModel ConfigureAppSettings<TSettingsModel>
		(this IServiceCollection services, IConfiguration configuration)
		where TSettingsModel : class, new()              
{
    if (configuration is null) throw new ArgumentNullException(nameof(configuration));

    var settingsModel = new TSettingsModel();
    configuration.Bind(settingsModel);

    services.AddSingleton(settingsModel);

    return settingsModel;
}
```

**步驟四**

在Startup.cs使用在步驟三所建立的擴充方法ConfigureAppSettings

```csharp
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    services.ConfigureAppSettings<MySettings>(Configuration.GetSection("MySettings"));
}
```

設定檔管理員建立完成 🎉

但如何使用設定檔管理員呢?

只需在建構子、方法...等地方注入MySettings，就可以直接取得設定檔資訊囉~

```csharp
public class AppSettingsDemoController : ControllerBase
{
    private readonly MySettings _mySettings;
    
    public AppSettingsDemoController(MySettings mySettings)
    {
        _mySettings = mySettings;  
    }

    [HttpGet("GetAccountByExtension")]
    public ActionResult<string> GetAccountByExtension()
    {
        return _mySettings.Account;
    }
}
```

[完整程式碼連結](https://github.com/melon1313/AppSettingsManager.git)

## 參考

[Strongly typed configuration in ASP.NET Core without IOptions<T>](https://www.strathweb.com/2016/09/strongly-typed-configuration-in-asp-net-core-without-ioptionst/)
