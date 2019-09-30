using System;
using System.Collections.Generic;
using System.Text;

namespace Selvagem
{
    public abstract class Animal
    {
        public Guid Id { get; } = Guid.NewGuid();
    }
}
