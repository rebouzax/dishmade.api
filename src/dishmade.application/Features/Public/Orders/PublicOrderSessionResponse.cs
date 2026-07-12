namespace dishmade.application.Features.Public.Orders;

public sealed record PublicOrderSessionResponse(
    bool WasCreated,
    bool WasRecovered,
    PublicOrderResponse Order
);