using System.Linq.Expressions;
using EventManager.Datalayer;
using EventManager.Datalayer.Dbos;
using EventManager.Models;
using FluentAssertions;

namespace RepositoryTests
{
    [TestFixture]
    public class DboConverterTests
    {
        private IDboConverter converter;
        private Guid id = Guid.Parse("00000000-0000-0000-0000-000000000001");

        [SetUp]
        public void Setup()
        {

            converter = new DboConverter();
        }

        [Test]
        public void Convert_Should_map_user_to_userDbo()
        {
            var user = new User
            {
                Id = id,
                Name = "Kamil",
                LastName = "Wiśniewski"
            };
            var expected = new UserDbo
            {
                Id = id,
                Name = "Kamil",
                LastName = "Wiśniewski"
            };

            var result = converter.Convert<UserDbo>(user);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_user_to_userDbo_when_only_id_has_value()
        {
            var user = new User
            {
                Id = id,
            };
            var expected = new UserDbo
            {
                Id = id,
            };

            var result = converter.Convert<UserDbo>(user);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userDbo_to_user()
        {
            var user = new UserDbo
            {
                Id = id,
                Name = "Kamil",
                LastName = "Wiśniewski"
            };
            var expected = new User
            {
                Id = id,
                Name = "Kamil",
                LastName = "Wiśniewski"
            };

            var result = converter.Convert<User>(user);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userDbo_to_user_when_only_id_has_value()
        {
            var user = new UserDbo
            {
                Id = id,
            };
            var expected = new User
            {
                Id = id,
            };

            var result = converter.Convert<User>(user);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_address_to_addressDbo()
        {
            var address = new Address
            {
                Id = id,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            var expected = new AddressDbo
            {
                Id = id,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };

            var result = converter.Convert<AddressDbo>(address);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_address_to_addressDbo_when_only_id_has_value()
        {
            var address = new Address
            {
                Id = id,
            };
            var expected = new AddressDbo
            {
                Id = id,
            };

            var result = converter.Convert<AddressDbo>(address);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_addressDbo_to_address()
        {
            var address = new AddressDbo
            {
                Id = id,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };
            var expected = new Address
            {
                Id = id,
                City = "Warszawa",
                Street = "Czerniakowska",
                HouseNumber = "24",
            };

            var result = converter.Convert<Address>(address);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_addressDbo_to_address_when_only_id_has_value()
        {
            var address = new AddressDbo
            {
                Id = id,
            };
            var expected = new Address
            {
                Id = id,
            };

            var result = converter.Convert<Address>(address);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_event_to_eventDbo()
        {
            var @event = new Event
            {
                Id = id,
                Name = "Sylwester",
                Description = "Sylwester dla przyjaciół",
                StartDate = DateTime.Parse("2025-12-31 20:00:00"),
                EndDate = DateTime.Parse("2026-01-01 04:00:00"),
                AddressId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Cost = 200,
                MaxParticipants = 50
            };
            var expected = new EventDbo
            {
                Id = id,
                Name = "Sylwester",
                Description = "Sylwester dla przyjaciół",
                StartDate = DateTime.Parse("2025-12-31 20:00:00"),
                EndDate = DateTime.Parse("2026-01-01 04:00:00"),
                AddressId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Cost = 200,
                MaxParticipants = 50
            };

            var result = converter.Convert<EventDbo>(@event);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_event_to_eventDbo_when_only_id_has_value()
        {
            var @event = new Event
            {
                Id = id,
            };
            var expected = new EventDbo
            {
                Id = id,
            };

            var result = converter.Convert<EventDbo>(@event);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_eventDbo_to_event()
        {
            var @event = new EventDbo
            {
                Id = id,
                Name = "Sylwester",
                Description = "Sylwester dla przyjaciół",
                StartDate = DateTime.Parse("2025-12-31 20:00:00"),
                EndDate = DateTime.Parse("2026-01-01 04:00:00"),
                AddressId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Cost = 200,
                MaxParticipants = 50
            };
            var expected = new Event
            {
                Id = id,
                Name = "Sylwester",
                Description = "Sylwester dla przyjaciół",
                StartDate = DateTime.Parse("2025-12-31 20:00:00"),
                EndDate = DateTime.Parse("2026-01-01 04:00:00"),
                AddressId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Cost = 200,
                MaxParticipants = 50
            };

            var result = converter.Convert<Event>(@event);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_eventDbo_to_event_when_only_id_has_value()
        {
            var @event = new EventDbo
            {
                Id = id,
            };
            var expected = new Event
            {
                Id = id,
            };

            var result = converter.Convert<Event>(@event);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userEvent_to_userEventDbo()
        {
            var userEvent = new UserEvent
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                DepositPaid = 100,
                JoinedAt = DateTime.Parse("2025-10-31 20:00:00"),
            };
            var expected = new UserEventDbo
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                DepositPaid = 100,
                JoinedAt = DateTime.Parse("2025-10-31 20:00:00"),
            };

            var result = converter.Convert<UserEventDbo>(userEvent);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userEvent_to_userEvent_when_only_id_has_value()
        {
            var userEvent = new UserEvent
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };
            var expected = new UserEventDbo
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };

            var result = converter.Convert<UserEventDbo>(userEvent);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userEventDbo_to_userEvent()
        {
            var userEvent = new UserEventDbo
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                DepositPaid = 100,
                JoinedAt = DateTime.Parse("2025-10-31 20:00:00"),
            };
            var expected = new UserEvent
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                DepositPaid = 100,
                JoinedAt = DateTime.Parse("2025-10-31 20:00:00"),
            };

            var result = converter.Convert<UserEvent>(userEvent);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Convert_Should_map_userEventDbo_to_userEvent_when_only_id_has_value()
        {
            var userEvent = new UserEventDbo
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };
            var expected = new UserEvent
            {
                UserId = id,
                EventId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
            };

            var result = converter.Convert<UserEvent>(userEvent);

            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void ConvertExpression_ShouldConvertUserExpressionToUserDboExpression()
        {
            Expression<Func<User, bool>> userExpression = u => u.Name == "John";

            var userDboExpression = converter.ConvertExpression<User, UserDbo>(userExpression);

            userDboExpression.Should().NotBeNull();
            userDboExpression.Parameters.Should().HaveCount(1);
            userDboExpression.Parameters[0].Type.Should().Be(typeof(UserDbo));
            var body = userDboExpression.Body as BinaryExpression;
            body.Should().NotBeNull();
            var left = body.Left as MemberExpression;
            left.Should().NotBeNull();
            left.Member.Name.Should().Be("Name");
        }
    }
}
