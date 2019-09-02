
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

[Route("api")]
[ApiController]
public class ApiController : ControllerBase
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

    [HttpGet("F")]
    public ActionResult<bool> F(Data data)
    {
        throw new TeapotException();
    }


    public class Data {
        [Required]
        public string Name { get; set; }
    }
}

[Serializable]
internal class TeapotException : Exception
{
    public TeapotException()
    {
    }

    public TeapotException(string? message) : base(message)
    {
    }

    public TeapotException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected TeapotException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}