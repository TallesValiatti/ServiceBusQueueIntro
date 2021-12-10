using Newtonsoft.Json;
using Receiver.Enuns;

namespace Receiver.Entities
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

        public static Payment DeserializeEntity(string content)
        {
            if(string.IsNullOrWhiteSpace(content))
                throw new Exception("Content is null, empty or whitespace");
            
            var obj = JsonConvert.DeserializeObject<Payment>(content);
            
            if(obj is null)
                throw new Exception("Deserialized object is not valid");
            
            return obj;
        }
    }    
}