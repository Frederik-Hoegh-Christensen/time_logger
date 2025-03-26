import { useEffect, useState } from "react";
import { projectService } from "~/api/projectService";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import { useProjectContext } from "~/contexts/projectContext";
import { TimeRegistrationProvider, useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";
import TimeRegistrationRows from "~/sharedComponents/timeRegistrationRows";
import CreateTimeRegistrationModal from "~/registerTime/components/createTimeRegistrationModal";
import WeekView from "~/registerTime/components/WeekView.tsx";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import NewEditModal from "~/sharedComponents/EditTimeRegistrationModal";
import { freelancerService } from "~/api/freelancerService";

//"DEA192EE-B1D0-43DA-0A7D-08DD6C71F515"

export default function TimeRegistration(){
  const {setProjects} = useProjectContext();
  const {timeRegistrations, setTimeRegistrations, selectedDate, setSelectedDate} = useTimeRegistrationContext();

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        // const projectsFromApi = await projectService.fetchProjectsByFreelancerId("DEA192EE-B1D0-43DA-0A7D-08DD6C71F515");
        const projectsFromApi = await freelancerService.getProjectsByFreelancerId("DEA192EE-B1D0-43DA-0A7D-08DD6C71F515");
        setProjects(projectsFromApi);
      } catch (error) {
        console.error("Error fetching projects:", error);
      }
    };
    fetchProjects();
    setSelectedDate(undefined);
    setTimeRegistrations([]);
  }, []);

  useEffect(() => {
    if (!selectedDate) return;
    const fetchTimeRegistrations = async () => {
      try {
        // const timeRegistrationsFromApi = await timeRegistrationService.fetchTimeRegistrationsByFreelancerIdAndDate("DEA192EE-B1D0-43DA-0A7D-08DD6C71F515", selectedDate!);
        const timeRegistrationsFromApi = await freelancerService.getTimeRegistrationsByFreelancerIdAndDate("DEA192EE-B1D0-43DA-0A7D-08DD6C71F515", selectedDate.toISOString().split("T")[0]);
        setTimeRegistrations([...timeRegistrationsFromApi]);
      } catch (error) {
        setTimeRegistrations([]);
        console.error("Error fetching time registrations:", error);
      }
    };
    fetchTimeRegistrations();
  }, [selectedDate]);

  const handleConfirmEdit = () => {
    setSelectedDate(new Date(selectedDate!))
  };

  const handleDelete = async (timeRegistrationId : string) => {
    await timeRegistrationService.deleteTimeRegistration(timeRegistrationId);
    setSelectedDate(new Date(selectedDate!))
  };
  return (
      
      <div >
          <WeekView/>
          {selectedDate && (
              <CreateTimeRegistrationModal timeRegistration={undefined}/>
          )}
          {timeRegistrations &&
              <TimeRegistrationRows onDelete={handleDelete} onConfirmEdit={handleConfirmEdit} timeRegistrations={timeRegistrations!}></TimeRegistrationRows>
          }
          
      </div>    
    );
}