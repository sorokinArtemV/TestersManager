using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class TestersService : ITestersService
{
    private readonly IDevStreamsService _devStreamsService;
    private readonly List<Tester> _testers;

    public TestersService(bool initialize = true)
    {
        _testers = new List<Tester>();
        _devStreamsService = new DevStreamsService();

        if (initialize)
            _testers.AddRange(new List<Tester>
                {
                    new()
                    {
                        TesterId = Guid.Parse("E83987F1-E884-446C-901F-978FC909BABF"),
                        TesterName = "Tanya Lightman",
                        DevStreamId = Guid.Parse("248A6FE4-AC09-452C-A205-A6CC4B7E9E56"),
                        Email = "rlightman0@uol.com.br",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1994-08-28"),
                        Position = "Middle QA",
                        MonthsOfWorkExperience = 3,
                        HasMobileDeviceExperience = true,
                        Skills = "JavaScript"
                    },
                    new()
                    {
                        TesterId = Guid.Parse("73452C7A-4206-499C-98FA-277407C2C23D"),
                        TesterName = "Tarrah McCard",
                        DevStreamId = Guid.Parse("1A76B36B-4B06-4A69-A368-7ADE27AB739E"),
                        Email = "tmccard1@webster.com",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1989-09-17"),
                        Position = "Senior QA",
                        MonthsOfWorkExperience = 30,
                        HasMobileDeviceExperience = false,
                        Skills = "Python"
                    },
                    new()
                    {
                        TesterId = Guid.Parse("6FF4BBBA-55E4-48B9-AED6-AD352D082E05"),
                        TesterName = "Alex Fruish",
                        DevStreamId = Guid.Parse("97BE8C70-E9AA-41D8-9BC6-F8832C1B485A"),
                        Email = "afruish2@multiply.com",
                        Gender = "Male",
                        BirthDate = DateTime.Parse("1998-07-30"),
                        Position = "G-ops",
                        MonthsOfWorkExperience = 36,
                        HasMobileDeviceExperience = false,
                        Skills = "Frs"
                    },
                    new()
                    {
                        TesterId = Guid.Parse("286E7C8D-759E-445A-9700-C82C15EE72C5"),
                        TesterName = "Marie Danev",
                        DevStreamId = Guid.Parse("02DF3B54-16F9-44C7-9272-C57873F8A2CA"),
                        Email = "bdanev3@posterous.com",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1999-07-07"),
                        Position = "Senior QA",
                        MonthsOfWorkExperience = 60,
                        HasMobileDeviceExperience = true,
                        Skills = "Frs"
                    },
                    new()
                    {
                        TesterId = Guid.Parse("518F9FEA-BF73-497D-A76F-EEC40204DAFA"),
                        TesterName = "Eleonore Asch",
                        DevStreamId = Guid.Parse("78FD1D57-28E2-4CD8-82A3-5DFDBA2A212A"),
                        Email = "easch5@upenn.edu",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1995-06-22"),
                        Position = "G-ops",
                        MonthsOfWorkExperience = 52,
                        HasMobileDeviceExperience = false,
                        Skills = "JavaScript"
                    },

                    new()
                    {
                        TesterName = "Arman Maty",
                        TesterId = Guid.Parse("957B658D-ED53-484B-9F9A-6D741657DECD"),
                        DevStreamId = Guid.Parse("1A76B36B-4B06-4A69-A368-7ADE27AB739E"),
                        Email = "amaty4@scribd.com",
                        Gender = "Male",
                        BirthDate = DateTime.Parse("1989-12-20"),
                        Position = "Middle QA",
                        MonthsOfWorkExperience = 9,
                        HasMobileDeviceExperience = false,
                        Skills = "CW"
                    },
                    new()
                    {
                        TesterName = "Alexander Padbery",
                        TesterId = Guid.Parse("3E2B5484-3A41-4F40-8126-BABBFB4B4CD2"),
                        DevStreamId = Guid.Parse("1A76B36B-4B06-4A69-A368-7ADE27AB739E"),
                        Email = "apadbery6@cloudflare.com",
                        Gender = "Male",
                        BirthDate = DateTime.Parse("1999-06-24"),
                        Position = "Junior QA",
                        MonthsOfWorkExperience = 20,
                        HasMobileDeviceExperience = false,
                        Skills = "Python"
                    },

                    new()
                    {
                        TesterName = "Shana Adame",
                        TesterId = Guid.Parse("9B379A26-9220-4BF1-BB70-193E2ADA3313"),
                        DevStreamId = Guid.Parse("97BE8C70-E9AA-41D8-9BC6-F8832C1B485A"),
                        Email = "sadame7@npr.org",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1986-05-08"),
                        Position = "Junior QA",
                        MonthsOfWorkExperience = 14,
                        HasMobileDeviceExperience = false,
                        Skills = "Python"
                    },
                    new()
                    {
                        TesterName = "Cherish Mumford",
                        TesterId = Guid.Parse("C3250D4A-CD91-4B73-A535-AC0E5BEDE0FA"),
                        DevStreamId = Guid.Parse("02DF3B54-16F9-44C7-9272-C57873F8A2CA"),
                        Email = "cmumford8@histats.com",
                        Gender = "Female",
                        BirthDate = DateTime.Parse("1989-05-01"),
                        Position = "Intern",
                        MonthsOfWorkExperience = 1,
                        HasMobileDeviceExperience = true,
                        Skills = "Blitz"
                    },
                    new()
                    {
                        TesterName = "Jason Detheridge",
                        TesterId = Guid.Parse("3C19DB1F-CDF0-486F-8D85-18A481C29D76"),
                        DevStreamId = Guid.Parse("78FD1D57-28E2-4CD8-82A3-5DFDBA2A212A"),
                        Email = "jdetheridge9@msn.com",
                        Gender = "Male",
                        BirthDate = DateTime.Parse("1991-09-24"),
                        Position = "Middle QA",
                        MonthsOfWorkExperience = 9,
                        HasMobileDeviceExperience = false,
                        Skills = "JavaScript"
                    }
                }
            );
    }

    public TesterResponse AddTester(TesterAddRequest? testerAddRequest)
    {
        ArgumentNullException.ThrowIfNull(testerAddRequest);
        ModelValidationHelper.IsValid(testerAddRequest);

        var tester = testerAddRequest.ToTester();
        tester.TesterId = Guid.NewGuid();

        _testers.Add(tester);

        return ConvertTesterToTesterResponse(tester);
    }

    public List<TesterResponse> GetAllTesters()
    {
        return _testers
            .Select(ConvertTesterToTesterResponse)
            .ToList();
    }

    public TesterResponse? GetTesterById(Guid? id)
    {
        return id is null
            ? null
            : _testers
                .Where(tester => tester.TesterId == id)
                .Select(ConvertTesterToTesterResponse)
                .FirstOrDefault();
    }

    public List<TesterResponse> GetFilteredTesters(string searchBy, string searchString)
    {
        var allTesters = GetAllTesters();
        var matchingTesters = allTesters;

        if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString)) return matchingTesters;

        matchingTesters = searchBy switch
        {
            nameof(TesterResponse.TesterName) =>
                FilterHelper.FilterBy(allTesters, x => x.TesterName, searchString),

            nameof(TesterResponse.Email) =>
                FilterHelper.FilterBy(allTesters, x => x.Email, searchString),

            nameof(TesterResponse.Gender) =>
                FilterHelper.FilterBy(allTesters, x => x.Gender, searchString),

            nameof(TesterResponse.DevStream) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStream, searchString),

            nameof(TesterResponse.Position) =>
                FilterHelper.FilterBy(allTesters, x => x.Position, searchString),

            nameof(TesterResponse.DevStreamId) =>
                FilterHelper.FilterBy(allTesters, x => x.DevStreamId?.ToString(), searchString),

            nameof(TesterResponse.Age) =>
                FilterHelper.FilterBy(allTesters, x => x.Age.ToString(), searchString),

            nameof(TesterResponse.BirthDate) =>
                FilterHelper.FilterBy(allTesters, x => x.BirthDate?.ToString("dd MMMM yyyy"), searchString),

            nameof(TesterResponse.MonthsOfWorkExperience) =>
                FilterHelper.FilterBy(allTesters, x => x.MonthsOfWorkExperience?.ToString(), searchString),

            nameof(TesterResponse.Skills) =>
                FilterHelper.FilterBy(allTesters, x => x.Skills, searchString),

            _ => matchingTesters
        };

        return matchingTesters;
    }


    public List<TesterResponse> GetSortedTesters(List<TesterResponse> allTesters, string sortBy,
        SortOrderOptions sortOrder)
    {
        return string.IsNullOrEmpty(sortBy) ? allTesters : SorterHelper.SortByProperty(allTesters, sortBy, sortOrder);
    }

    public TesterResponse UpdateTester(TesterUpdateRequest? testerUpdateRequest)
    {
        ArgumentNullException.ThrowIfNull(testerUpdateRequest);
        ModelValidationHelper.IsValid(testerUpdateRequest);

        var tester = _testers.FirstOrDefault(tester => tester.TesterId == testerUpdateRequest.TesterId);

        ArgumentNullException.ThrowIfNull(tester);

        tester.TesterName = testerUpdateRequest.TesterName;
        tester.Email = testerUpdateRequest.Email;
        tester.Gender = testerUpdateRequest.Gender.ToString();
        tester.BirthDate = testerUpdateRequest.BirthDate;
        tester.DevStreamId = testerUpdateRequest.DevStreamId;
        tester.Position = testerUpdateRequest.Position;
        tester.MonthsOfWorkExperience = testerUpdateRequest.MonthsOfWorkExperience;
        tester.HasMobileDeviceExperience = testerUpdateRequest.HasMobileDeviceExperience;
        tester.Skills = string.Join(", ", testerUpdateRequest.Skills);

        return ConvertTesterToTesterResponse(tester);
    }

    public bool DeleteTester(Guid? testerId)
    {
        ArgumentNullException.ThrowIfNull(testerId);

        var tester = _testers.FirstOrDefault(x => x.TesterId == testerId);

        if (tester is null) return false;

        _testers.RemoveAll(x => x.TesterId == testerId);

        return true;
    }

    private TesterResponse ConvertTesterToTesterResponse(Tester tester)
    {
        var testerResponse = tester.ToTesterResponse();
        testerResponse.DevStream = _devStreamsService.GetDevStreamById(testerResponse.DevStreamId)?.DevStreamName;
        return testerResponse;
    }
}