﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewDatingApp.API.Data;

namespace NewDatingApp.API.Controllers
{
    // http://locqlhost:5000/api/
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly DataContext context;
        public ValuesController(DataContext context)
        {
            this.context = context;

        }
        
        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetValues()
        {
            var values = await this.context.Values.ToListAsync();
            return Ok(values);
            //return new string[] { "value1", "value2", "Ventinga", "elementarysoft", "Mirko" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var value = await this.context.Values.FirstOrDefaultAsync(v => v.Id == id);
            return Ok(value);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
