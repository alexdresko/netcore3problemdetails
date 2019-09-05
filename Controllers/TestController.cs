
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Hellang.Middleware.ProblemDetails;
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

    [HttpGet("B2")]
    public ActionResult B2(Data data)
    {
        ModelState.AddModelError("", "for reals");
        var validation = new ValidationProblemDetails(ModelState);
        throw new ProblemDetailsException(validation);
    }

    [HttpGet("B3")]
    public ActionResult B3()
    {
        ModelState.AddModelError("", "for reals");
        var validation = new ValidationProblemDetails(ModelState);
        throw new ProblemDetailsException(validation);
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

    public class Deets : ProblemDetails
    {
        public string Name { get; set; }
    }

    [HttpGet("D2")]
    public ActionResult<bool> D2(Data data)
    {
        return BadRequest(new Deets { Name = "Fred" });
    }

    [HttpGet("D3")]
    public ActionResult<bool> D3()
    {
        return NotFound(3);
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

    [HttpGet("G")]
    public ActionResult<bool> G()
    {
        // if some error condition...
        return Problem(title: "An error occurred while processing your request", statusCode: 400);
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