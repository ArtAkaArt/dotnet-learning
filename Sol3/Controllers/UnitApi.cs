using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using Sol0;

namespace Sol3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UnitApi : ControllerBase
    {
        private string login;
        private string pwd;

        public UnitApi(IOptions<AccountB> acc)
        {
            login = acc.Value.AccName;
            pwd = acc.Value.Password;
        }
        /// <summary>
        /// ��������� ���� ������
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public ActionResult GetAll()
        {
            return Ok();
        }
        /// <summary>
        /// ��������� ����� �� id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult GetById([FromRoute] string id)
        {
            return Ok(id);
        }
        /// <summary>
        /// ���������� ����� ���������
        /// </summary>
        /// <param name="unit">Json �����</param>
        /// <returns></returns>
        [HttpPost(Name = "Add")]
        public ActionResult AddUnit([FromBody] Unit unit)
        {
            return Ok(unit.Name);
        }
        /// <summary>
        /// �������������� ���������
        /// </summary>
        /// <param name="id"> id ����� ��� ���������</param>
        /// <param name="unit"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult ReplaceById([FromRoute] string id, [FromBody] Unit unit)
        {
            return Ok($"������ ���� �� {id}, �� {unit.Name}");
        }
        /// <summary>
        /// �������� ��������� �� ����� ������������
        /// </summary>
        /// <param name="id">id �����</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult DeleteById([FromRoute] string id)
        {
            return Ok($"����� ������ ���� � {id}");
        }
    }
}