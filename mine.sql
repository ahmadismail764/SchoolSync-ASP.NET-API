select
    schoolyears.id as scholyearid,
    year,
    name,
    SchoolYears.IsActive as schoolyearactive
from
    schoolyears
    left join schools on schools.id = schoolyears.schoolId;