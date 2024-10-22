using PayrollSystem.Application.DTOs;
using PayrollSystem.Application.Interfaces;
using PayrollSystem.Domain.Entities;

namespace PayrollSystem.Application.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<IEnumerable<DepartmentDto>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            return departments.Select(d => MapToDto(d));
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            return department != null ? MapToDto(department) : null;
        }

        public async Task<DepartmentDto> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            var department = MapToEntity(departmentDto);
            await _departmentRepository.AddAsync(department);
            return MapToDto(department);
        }

        public async Task UpdateDepartmentAsync(int id, DepartmentDto departmentDto)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                throw new KeyNotFoundException($"Department with ID {id} not found.");
            }

            UpdateEntityFromDto(department, departmentDto);
            await _departmentRepository.UpdateAsync(department);
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            await _departmentRepository.DeleteAsync(id);
        }

        private DepartmentDto MapToDto(Department department)
        {
            return new DepartmentDto
            {
                Id = department.Id,
                Name = department.Name,
                Code = department.Code
            };
        }

        private Department MapToEntity(DepartmentDto dto)
        {
            return new Department
            {
                Name = dto.Name,
                Code = dto.Code
            };
        }

        private void UpdateEntityFromDto(Department department, DepartmentDto dto)
        {
            department.Name = dto.Name;
            department.Code = dto.Code;
        }
    }
}
