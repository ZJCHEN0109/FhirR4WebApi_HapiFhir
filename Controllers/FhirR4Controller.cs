using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using System.Text.Json;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Rest;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Day4WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FhirR4Controller : ControllerBase
    {
        //Data Field
        private IHttpClientFactory _factory;
        //DI(Dependency Injection) 
        public FhirR4Controller(IHttpClientFactory factory)
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
        public async Task<IActionResult> seachFhirR4()
        {
            //1.需要一個HttpClient物件 進行遠端服務請求 配合向前向後相容性
            HttpClient client = _factory.CreateClient();
            //設定請求遠端服務位址
            client.BaseAddress = new Uri("http://hapi.fhir.org/baseR4/");
            //正式請求 讀取Json文件回來
            String jsonString = client.GetStringAsync("Patient?_format=json").GetAwaiter().GetResult();
            //String jsonString = await client.GetStringAsync("");
            //Jtoken取得JSON讀取陣列
            JObject objJObject = JObject.Parse(jsonString);
            JToken dataList = objJObject.SelectToken("entry");
            List<JToken> datachildList = dataList.Children().ToList();

            //取得JSON內個別資料
            //foreach (JToken datajson in dataList)
            //{
            //    string line = string.Format("{0}/{1}/{2}",
            //    datajson.SelectToken("resource").Value<string>("name.family"),
            //    datajson.SelectToken("resource").Value<string>("gender"),
            //    datajson.SelectToken("resource").Value<string>("birthDate"));
            //    line += line + "\r\n";
            //    Console.WriteLine(line);
            //};

            //查詢for Object(List) 採用LINQ(LINK) 整合查詢語言
            var result = from u in datachildList
                         select u;
            string line2 = "";
            foreach (var item in result)
            {
                string line = string.Format("{0}", item);
                line2 += line + "\r\n";
                Console.WriteLine(item);
            }
            //List<Models.FhirR4.Entry> data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.FhirR4.Entry>>(jsonString);
            //return this.Ok(result.ToList<Models.FhirR4.Entry>());
            //JToken method
            Models.FhirR4.Root fhirresult = new Models.FhirR4.Root();
            return this.Ok(result.ToList<Models.FhirR4.Root>());
        }

    }
}
