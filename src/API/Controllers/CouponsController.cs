using System.Collections.ObjectModel;
using API.Errors;
using Core.DTOs.CouponDTOs;
using Core.Interfaces.IDomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


/// <response code="500">If there is an internal server error.</response>
[ApiController]
[Route("api/coupons")]
[Produces("application/json")]
[ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
public class CouponsController : ControllerBase
{
    private readonly ICouponsService brandsService;

    public CouponsController(ICouponsService brandsService)
    {
        this.brandsService = brandsService;
    }


    [HttpGet]
    [ProducesResponseType(typeof(ReadOnlyCollection<CouponResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCoupons()
    {
        var result = await brandsService.GetCoupons();

        return Ok(result);
    }


    [HttpGet("{id:Guid}")]
    [ProducesResponseType(typeof(CouponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCoupon(Guid id)
    {
        var result = await brandsService.GetCoupon(id);

        return Ok(result);
    }


    /// <summary>
    /// Will help you to check if the discount code provided by the customer is valid or not.
    /// </summary>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(CouponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCoupon(string code)
    {
        var result = await brandsService.GetCoupon(code);

        return Ok(result);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the created coupon.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    /// <response code="409">If the code is already used by another coupon.</response>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CouponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddCoupon(CouponAddRequest couponAddRequest)
    {
        var createdCoupon = await brandsService.AddCoupon(couponAddRequest);

        return CreatedAtAction(nameof(GetCoupon), new { id = createdCoupon.Id }, createdCoupon);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="200">Returns the updated coupon.</response>
    /// <response code="404">If the coupon you are trying to update is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpPut("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(CouponResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateCoupon(Guid id, CouponUpdateRequest couponUpdateRequest)
    {
        var updatedCoupon = await brandsService.UpdateCoupon(id, couponUpdateRequest);

        return Ok(updatedCoupon);
    }


    /// <summary>
    /// (Can be called by admins only)
    /// </summary>
    /// <response code="204">If the coupon has been deleted sucessfully.</response>
    /// <response code="404">If the coupon you are trying to delete is not found.</response>
    /// <response code="401">If there is no access token has been provided with the request.</response>
    /// <response code="403">If the access token has been provided but the user is not an admin.</response>
    [HttpDelete("{id:Guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> DeleteCoupon(Guid id)
    {
        await brandsService.DeleteCoupon(id);

        return NoContent();
    }
}