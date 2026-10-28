using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarInsurance.Models;
using CarInsurance.Data;

public class InsureesController : Controller
{
    private readonly InsuranceContext _context;

    public InsureesController(InsuranceContext context)
    {
        _context = context;
    }

    // GET: INSUREES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Insurees.ToListAsync());
    }

    public async Task<IActionResult> Admin()
    {
        return View(await _context.Insurees.ToListAsync());
    }

    // GET: INSUREES/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // GET: INSUREES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: INSUREES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (ModelState.IsValid)
        {
            insuree.Quote = calculateQuote(insuree);
            _context.Add(insuree);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(insuree);
    }

    // GET: INSUREES/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree == null)
        {
            return NotFound();
        }
        return View(insuree);
    }

    // POST: INSUREES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid? id, [Bind("Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
    {
        if (id != insuree.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                insuree.Quote = calculateQuote(insuree);
                _context.Update(insuree);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsureeExists(insuree.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(insuree);
    }

    // GET: INSUREES/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var insuree = await _context.Insurees
            .FirstOrDefaultAsync(m => m.Id == id);
        if (insuree == null)
        {
            return NotFound();
        }

        return View(insuree);
    }

    // POST: INSUREES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid? id)
    {
        var insuree = await _context.Insurees.FindAsync(id);
        if (insuree != null)
        {
            _context.Insurees.Remove(insuree);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool InsureeExists(Guid? id)
    {
        return _context.Insurees.Any(e => e.Id == id);
    }

    private decimal calculateQuote(Insuree insuree)
    {
        //Base = 50
        decimal quote = 50m;

        //Calculate age based of Birthday and current date
        int age = calculateAge(insuree.DateOfBirth);

        //if age <=18 + 100 to monthly total
        if (age <= 18)
        {
            quote += 100;
        }
        else if (age >= 19 && age <= 25)//else if age >= 19 and <= 25 + 50 to monthly total
        {
            quote += 50;
        }
        else if (age >= 26) //else if age >= 26 + 25
        {
            quote += 25;
        }
        
        //if car year < 2000 + 25 or if car year > 2015 + 25
        if (insuree.CarYear < 2000 || insuree.CarYear > 2015)
        {
            quote += 25;
        }

        //if car make == "Porsche" + 25
        if (insuree.CarMake == "Porsche")
        {
            quote += 25;

            //if car model == "911 Carrera" + 25
            if(insuree.CarModel == "911 Carrera")
            {
                quote += 25;
            }
        }

        //if speeding ticket > 0 total monthly + (10 * # of speeding tickets)
        if (insuree.SpeedingTickets > 0) 
        {
            quote += (10 * insuree.SpeedingTickets);
        }

        //if DUI monthly total * 1.25
        if (insuree.DUI)
        {
            quote *= 1.25m;
        }

        //if coverage monthly total * 1.5
        if (insuree.CoverageType)
        {
            quote *= 1.5m;
        }
        return quote;
    }

    private static int calculateAge(DateTime dateOfBirth)
    {
        DateTime currentDate = DateTime.Now;
        int age = currentDate.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > currentDate.AddYears(-age))
        {
            age--;
        }

        return age;
    }
}
