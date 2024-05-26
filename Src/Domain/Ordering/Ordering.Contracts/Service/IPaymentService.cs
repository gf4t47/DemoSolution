// Copyright (c) Demo.
namespace Ordering.Service;

using System.Threading.Tasks;
using Ordering.Model;
public interface IPaymentService
{
    Task<bool> Pay(int amount, PaymentInfo paymentInfo);
}
