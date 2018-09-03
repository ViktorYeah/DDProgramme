﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubleMastersDegreeProgramme.Models
{
    public class FileModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string FileType { get; set; }
        public string Description { get; set; }
    }
}
