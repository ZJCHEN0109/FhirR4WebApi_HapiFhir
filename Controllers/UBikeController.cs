using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Day4WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UBikeController : ControllerBase
    {
        //Data Field
        private IHttpClientFactory _factory;
        //DI(Dependency Injection) 
        public UBikeController(IHttpClientFactory factory)
        {
            _factory = factory;
        }

        //Action 驗證工廠是否被注入
        [HttpGetAttribute]
        [ProducesAttribute("text/plain")]
        public IActionResult validHttpFactory()
        {
            return this.Ok(_factory.ToString());
        }

        //串接Ubike Service 透過區域查詢基座狀態
        [HttpGetAttribute]
        [ProducesAttribute("application/json")]
        public async Task<IActionResult> seachUbike([FromQuery]String sarea)
        {
            //1.需要一個HttpClient物件 進行遠端服務請求 配合向前向後相容性
            HttpClient client=_factory.CreateClient();
            //設定請求遠端服務位址
            client.BaseAddress = new Uri("https://tcgbusfs.blob.core.windows.net/dotapp/youbike/v2/youbike_immediate.json");
            //正式請求 讀取Json文件回來
            //String jsonString=client.GetStringAsync("").GetAwaiter().GetResult();
            String jsonString=await client.GetStringAsync("");
            List<Models.Ubike> data=Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.Ubike>>(jsonString);
            //查詢for Object(List) 採用LINQ(LINK) 整合查詢語言
            var result = from u in data
                         where u.sarea == sarea
                         select u;
            //Lazy Query
            return this.Ok(result.ToList<Models.Ubike>());
        }

    }
}
