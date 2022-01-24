using EFCore_Tutorial.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EFCore_Tutorial.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly DataContext _context;
        public CharacterController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Character>>> Get(int userID)
        {            
            var characters = await _context.Characters
                .Where(c=> c.UserId == userID)
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .ToListAsync();

            return characters;
        }

        [HttpPost]
        public async Task<ActionResult<List<Character>>> Create(CreateCharacterDto request)
        {
            var user = await _context.Users.FindAsync(request.UserId);
            if(user == null)
                return NotFound();

            var newCharacter = new Character
            {
                Name = request.Name,
                RpgClass = request.RpgClass,
                User = user
            };                   
            
            return await Get(newCharacter.UserId);
        }


        [HttpPost("weapon")]
        public async Task<ActionResult<Character>> AddWeapon(AddWeaponDto request)
        {
            var character = await _context.Characters.FindAsync(request.CharacterID);
            if (character == null)
                return NotFound();

            var newWeapon = new Weapon
            {
                Name = request.Name,
                Damage = request.Damage,
                Character = character
            };

            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();

            return character;
        }


        [HttpPost("skill")]
        public async Task<ActionResult<Character>> AddCharacterSkill(AddCharacterSkillDto request)
        {
            var character = await _context.Characters
                .Where(c => c.Id == request.CharacterID)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync();

            if (character == null)
                return NotFound();

            var skill = await _context.Skills.FindAsync(request.SkillId);
            if (skill == null)
                return NotFound();

            character.Skills.Add(skill);
            await _context.SaveChangesAsync();

            return character;
        }

    }
}
