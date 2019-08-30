
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

[Route("api")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("A")]
    public ActionResult A(Data data) {
        throw new System.Exception("testoo");
    }

    [HttpGet("B")]
    public ActionResult B(Data data)
    {
        ModelState.AddModelError("", "for reals");
        return this.ValidationProblem();
    }

    [HttpGet("C")]
    public ActionResult C(Data data)
    {
        // force a div by zero exception
        var x = 0;
        x = 0 / x;
        return Ok();
    }

    [HttpGet("D")]
    public ActionResult<bool> D(Data data)
    {
        return BadRequest();
    }

    [HttpGet("E")]
    public ActionResult<bool> E(Data data)
    {
        ModelState.AddModelError("", "for reals");
        return BadRequest(ModelState);
    }


    public class Data {
        [Required]
        public string Name { get; set; }
    }
}