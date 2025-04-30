using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Mapster;

namespace Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext.Coupons.FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        coupon ??= new Coupon() { ProductName = "No Discount", Amount = 0, Description = "Not Exist" };

        var couponModel = coupon.Adapt<CouponModel>();

        logger.LogInformation("Discount is successfully Get. ProductName : {ProductName}", request.ProductName);

        return couponModel;

    }

    public override async Task<CouponModel> CreateDicount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(CreateStatus(StatusCode.InvalidArgument, "Invalid Request"));
        dbContext.Coupons.Add(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully Created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>() ?? throw new RpcException(CreateStatus(StatusCode.InvalidArgument, "Invalid Request"));

        dbContext.Coupons.Update(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully Created. ProductName : {ProductName}", coupon.ProductName);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {

        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName) ??
            throw new RpcException(CreateStatus(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));



        dbContext.Coupons.Remove(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully Deleted. ProductName : {ProductName}", coupon.ProductName);


        return new DeleteDiscountResponse { Success = true };
    }

    private static Status CreateStatus(StatusCode s, ReadOnlySpan<char> details) => new(s, details.ToString());
}
