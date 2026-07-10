using System;
using System.Threading.Tasks;
using FluentAssertions;
using JetBrains.Annotations;
using Xunit;

namespace Light.GuardClauses.Tests.Issues
{
    public static class Issue66
    {
        [Fact]
        public static async Task ReSharperShowsSquigglyLinesAlthoughMustNotBeNullWasCalledBeforehand()
        {
            var repo = new SomeRepositoryMock();
            repo.Entity = null; // If you comment out this line, the squiggly lines will not be shown
            var logic = new CoreLogic(() => repo);

            var entity = await logic.GetIt();

            var savedEntity = repo.Entity;
            savedEntity.MustNotBeNull(); // This call should actually ensure that savedEntity is not null
            //savedEntity = savedEntity.MustNotBeNull(); // If you uncomment this line and comment the one above, then the squiggly lines also disappear
            entity.Id.Should().Be(savedEntity.Id); // These squiggly lines should not be there when dereferencing savedEntity
        }
    }

    public static class Assertions
    {
        [ContractAnnotation("parameter:null => halt; parameter:notnull => notnull")]
        public static T MustNotBeNull<T>(this T parameter) where T : class
        {
            if (parameter == null)
                throw new ArgumentNullException();
            return parameter;
        }
    }

    public sealed class Entity
    {
        public string Id { get; set; }

        public static Entity CreateDefault() => new() { Id = "Foo" };
    }

    public interface ISomeRepository : IDisposable
    {
        Task<Entity> GetInstanceAsync();

        Task SaveInstanceAsync(Entity dto);
    }

    public sealed class CoreLogic
    {
        private readonly Func<ISomeRepository> _createRepo;

        public CoreLogic(Func<ISomeRepository> createRepo) => _createRepo = createRepo;

        public async Task<Entity> GetIt()
        {
            using var repo = _createRepo();
            var instance = await repo.GetInstanceAsync();
            if (instance == null)
            {
                instance = Entity.CreateDefault();
                await repo.SaveInstanceAsync(instance);
            }

            return instance;
        }
    }

    public sealed class SomeRepositoryMock : ISomeRepository
    {
        public Entity Entity;

        public Task<Entity> GetInstanceAsync() => Task.FromResult(Entity);

        public Task SaveInstanceAsync(Entity dto)
        {
            Entity = dto;
            return Task.CompletedTask;
        }

        public void Dispose() { }
    }
}

namespace JetBrains.Annotations
{
    [AttributeUsage(
        AttributeTargets.Method | AttributeTargets.Parameter | AttributeTargets.Property |
        AttributeTargets.Delegate | AttributeTargets.Field | AttributeTargets.Event |
        AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.GenericParameter)]
    internal sealed class NotNullAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    internal sealed class ContractAnnotationAttribute : Attribute
    {
        public ContractAnnotationAttribute([NotNull] string contract)
            : this(contract, false) { }

        public ContractAnnotationAttribute([NotNull] string contract, bool forceFullStates)
        {
            Contract = contract;
            ForceFullStates = forceFullStates;
        }

        [NotNull]
        public string Contract { get; }

        public bool ForceFullStates { get; }
    }
}