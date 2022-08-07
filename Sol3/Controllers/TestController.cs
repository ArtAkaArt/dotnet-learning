using Microsoft.AspNetCore.Mvc;
using Sol3.Profiles;

namespace Sol3.Controllers
{
    public class ATestController : ControllerBase
    {
        /// <summary>
        /// отправка 5 аргументов, 1 с моим атрибутом (0 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test1")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test1
                        (Obj2 o1, Obj3 o2, Obj4 o3, Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 1 с моим атрибутом (1 не валидный)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test2")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test2
            ( Obj1 o1, Obj3 o2,  Obj4 o3, Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 2 с моим атрибутом (0 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test3")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test3
                        (Obj2 o1, Obj6 o2, Obj4 o3, Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 2 с моим атрибутом (1 не валидный)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test4")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test4
                        ( Obj2 o1,  Obj1 o2,  Obj4 o3,  Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 2 с моим атрибутом (2 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test5")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test5
                        (Obj1 o1, Obj5 o2, Obj4 o3, Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        
        /// <summary>
        /// отправка 5 аргументов, 3 с моим атрибутом (1 не валидный)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test6")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test6
                        ( Obj2 o1,  Obj6 o2,  Obj1 o3,  Obj3 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 3 с моим атрибутом (2 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test7")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test7
                        ( Obj2 o1,  Obj5 o2,  Obj1 o3, Obj3 o4,  Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 4 с моим атрибутом (0 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test8")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test8
                        (Obj2 o1, Obj2 o2, Obj6 o3, Obj6 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 4 с моим атрибутом (1 не валидный)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test9")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test9
                        ( Obj2 o1,  Obj2 o2,  Obj6 o3,  Obj1 o4,  Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 4 с моим атрибутом (2 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test10")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test10
                        (Obj6 o1, Obj2 o2, Obj5 o3, Obj1 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 4 с моим атрибутом (3 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test11")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test1
                        (Obj2 o1, Obj1 o2, Obj5 o3, Obj1 o4, Obj4 o5)
        {
            //Проверка аттрибутов
            return Ok();
        }
        /// <summary>
        /// отправка 5 аргументов, 4 с моим атрибутом (4 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test12")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test12
                        (Obj1 o1 , Obj1 o2 ,Obj5 o3, Obj5 o4, Obj4 o5)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 1 аргумента(0 не валидных)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test13")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test13
                        (Obj2 o1)
        {
            return Ok();
        }
        /// <summary>
        /// отправка 1 аргумента(1 не валидный)
        /// </summary>
        /// <returns></returns>
        [HttpPost("Test14")] // , Authorize
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Test14
                        (Obj1 o1)
        {
            return Ok();
        }
    }
}
