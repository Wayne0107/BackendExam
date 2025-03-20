using BackendExam.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BackendExam.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BackendExamtController : ControllerBase
    {
        private readonly BackendExamHubContext _context;
        private readonly ILogger<BackendExamtController> _logger;
        private readonly IBackendExamHubContextProcedures _procedures;

        public BackendExamtController(BackendExamHubContext context, ILogger<BackendExamtController> logger, IBackendExamHubContextProcedures procedures)
        {
            _context = context;
            _logger = logger;
            _procedures = procedures;
        }

        /// <summary>
        /// 查詢使用者
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            OutputParameter<string> logResult = new OutputParameter<string>();

            // 調用存儲過程
             List <usp_GetMyoffice_ACPDResult> result = await _procedures.usp_GetMyoffice_ACPDAsync(id);


            // 執行 Log 預存程序
            await _procedures.usp_AddLogAsync(
                1, "usp_GetMyoffice_ACPD", Guid.NewGuid(), "查詢使用者",
                Newtonsoft.Json.JsonConvert.SerializeObject(result),
                logResult
            );



            return Ok(result);
        }

        /// <summary>
        /// 新增使用者 (Create)
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MyOfficeAcpdDTO myOfficeAcpdDTO)
        {
            try
            {
                OutputParameter<int> result = new OutputParameter<int>();

                await _procedures.usp_AddMyoffice_ACPDAsync(JsonConvert.SerializeObject(myOfficeAcpdDTO));
                //// 執行 Log 預存程序
                //await _procedures.usp_AddLogAsync(
                //    1, "usp_AddMyoffice_ACPD", Guid.NewGuid(), "新增使用者",
                //    Newtonsoft.Json.JsonConvert.SerializeObject(result),
                //    result.ToString()
                //);
                return CreatedAtAction("Post", new { id = myOfficeAcpdDTO.ACPD_SID }, myOfficeAcpdDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Error");
                return NotFound();
            }

        }

        /// <summary>
        /// 更新使用者 (Update)
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] MyOfficeAcpdDTO myOfficeAcpdDTO)
        {
            try
            {
                OutputParameter<int> result = new OutputParameter<int>();
                await _procedures.usp_UpdateMyoffice_ACPDAsync(JsonConvert.SerializeObject(myOfficeAcpdDTO));
                return Ok(myOfficeAcpdDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Error");
                return NotFound();
            }

        }

        /// <summary>
        /// 刪除使用者 (Delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                OutputParameter<int> result = new OutputParameter<int>();
                await _procedures.usp_DeleteMyoffice_ACPDAsync(id, result);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Delete Error");
                return NotFound();
            }

        }
    }
}
