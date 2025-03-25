import { useEffect, useState } from "react";
import { projectService } from "~/api/projectService";
import CreateProjectModal from "~/projectOverview/components/createProjectModal";
import { useProjectContext } from "~/contexts/projectContext";
import ProjectRow from "~/projectOverview/components/projectRow";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronDown, faChevronUp } from "@fortawesome/free-solid-svg-icons";
import EditTimeRegistrationModal from "~/sharedComponents/EditTimeRegistrationModal";



export default function projectOverview() {
  const {projects, setProjects, selectedTimeRegistration } = useProjectContext();
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
  }, []);


  const sortProjectsAsc = () => {
    const sortedProjects = [...projects].sort((a, b) => {
      const dateA = new Date(a.deadline.replace(/(\d{4})\/(\d{2})\/(\d{2})/, "$1-$3-$2"));
      const dateB = new Date(b.deadline.replace(/(\d{4})\/(\d{2})\/(\d{2})/, "$1-$3-$2"));
      
      return dateA.getTime() - dateB.getTime();
    });
    setProjects(sortedProjects)

  }

  const sortProjectsDesc = () => {
    const sortedProjects = [...projects].sort((a, b) => {
      const dateA = new Date(a.deadline.replace(/(\d{4})\/(\d{2})\/(\d{2})/, "$1-$3-$2"));
      const dateB = new Date(b.deadline.replace(/(\d{4})\/(\d{2})\/(\d{2})/, "$1-$3-$2"));
      
      return dateB.getTime() - dateA.getTime() ;
    });
    setProjects(sortedProjects)

  }
  

  return (
    <div style={{display: 'flex', alignItems: 'center', width: '100%', marginTop: 100, flexDirection: 'column'}}>
      <CreateProjectModal/>
      {selectedTimeRegistration && <EditTimeRegistrationModal/>}
      
      <div style={{display:'flex', flexDirection: 'row', paddingTop: 20}}>
        <FontAwesomeIcon icon={faChevronDown} onClick={sortProjectsAsc} cursor='pointer' />
        <FontAwesomeIcon icon={faChevronUp} onClick={sortProjectsDesc} cursor='pointer'/>
      </div>
      {projects && projects.length > 0 && (
        projects.map(p => (
          <ProjectRow project={p}/>
        ))
      )}
    </div>
    

  );
}