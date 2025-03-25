import { useEffect, useState } from "react";
import { projectService } from "~/api/projectService";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import { useProjectContext } from "~/contexts/projectContext";
import { TimeRegistrationProvider, useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";
import TimeRegistrationRows from "~/sharedComponents/timeRegistrationRows";
import CreateTimeRegistrationModal from "~/registerTime/components/createTimeRegistrationModal";
import WeekView from "~/registerTime/components/WeekView.tsx";
import EditTimeRegistrationModal from "~/sharedComponents/EditTimeRegistrationModal";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import NewEditModal from "~/sharedComponents/NewEditModal";

//"5088F034-274E-412B-2394-08DD648C3E34"

export default function TimeRegistration(){
  const {setProjects} = useProjectContext();
  const {timeRegistrations, setTimeRegistrations, selectedDate, setSelectedDate} = useTimeRegistrationContext();
  const {openEditTimeRegistrationModal, setOpenEditTimeRegistrationModal, selectedTimeRegistration} = useProjectContext();
  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const projectsFromApi = await projectService.fetchProjectsByFreelancerId("5088F034-274E-412B-2394-08DD648C3E34");
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
        const timeRegistrationsFromApi = await timeRegistrationService.fetchTimeRegistrationsByFreelancerIdAndDate("5088F034-274E-412B-2394-08DD648C3E34", selectedDate!);
        setTimeRegistrations([...timeRegistrationsFromApi]);
      } catch (error) {
        setTimeRegistrations([]);
        console.error("Error fetching projects:", error);
      }
    };
    fetchTimeRegistrations();
  }, [selectedDate]);

  const handleEdit = () => {
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
              <TimeRegistrationRows onDelete={handleDelete} onEdit={handleEdit} timeRegistrations={timeRegistrations!}></TimeRegistrationRows>
          }
          
      </div>    
    );
}