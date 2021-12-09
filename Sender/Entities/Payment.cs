using Newtonsoft.Json;
using Sender.Enuns;

namespace Sender.Entities
{
    public class Payment
    {
        public double Value { get; private set; }
        public PaymentType PaymentType { get; private set; }

        public Payment(double value, PaymentType paymentType)
        {
            this.Value = value;
            this.PaymentType = paymentType;
        }

        public string SerializeEntity()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}