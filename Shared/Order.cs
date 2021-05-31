using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    // Data class die onze order status bevat.
    // bevind zich in het Shared project omdat ze toegankelijk moet zijn vanuit OrderGenerator en OrderConsumer
    
    [Serializable] // Omdat we dit object gaan serialiseren moet dit attribuut voorzien worden.
    public class Order
    {
        //Properties
        public bool Status { get; set; } = false;
    }
}
