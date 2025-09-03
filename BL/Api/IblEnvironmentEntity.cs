using BL.Models;

namespace BL.Api
{
    public interface IBlEnvironmentEntity
    {

        Task<List<BlEnvironmentEntity>> GetAll();
        Task<BlEnvironmentEntity> GetById(int id);
        Task<BlEnvironmentEntity> Create(BlEnvironmentEntity item);
        Task<BlEnvironmentEntity> Update(BlEnvironmentEntity item);
        Task Delete(int id);

        // �������� ���� ��� �����

        BlEnvironmentEntity CastingEnvironmentEntityFromBlToDal(BlEnvironmentEntity? e);

    }
}
