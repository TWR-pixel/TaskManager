﻿using TaskManager.Core.Entities.Common;
using TaskManager.Core.Entities.Tasks;

namespace TaskManager.Core.Entities.TaskColumns;

public class TaskColumnEntity : EntityBase
{
    public required string Name { get; set; }
}
