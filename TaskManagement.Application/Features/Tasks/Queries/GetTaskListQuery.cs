﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Features.Tasks.Queries
{
    public class GetTaskListQuery : IRequest<List<TodoTask>>
    {
    }
}
