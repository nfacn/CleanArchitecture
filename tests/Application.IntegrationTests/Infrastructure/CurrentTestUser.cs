using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanArchitecture.Application.Common.Interfaces;

namespace CleanArchitecture.Application.IntegrationTests.Infrastructure;

public class CurrentTestUser : IUser
{
    public string? Id { get; set; }
}
