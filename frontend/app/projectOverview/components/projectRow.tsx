import { useEffect, useState } from "react";
import type { ProjectDTO } from "~/models/project";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronDown, faChevronUp } from '@fortawesome/free-solid-svg-icons';
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import TimeRegistrationRows from "../../sharedComponents/timeRegistrationRows";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import { useProjectContext } from "~/contexts/projectContext";
import { useTimeRegistrationContext } from "~/contexts/timeRegistrationContext";

interface IProps {
    project: ProjectDTO;
}
const ProjectRow: React.FC<IProps> = ({ project}) => {
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [timeRegistrations, setTimeRegistrations] = useState<TimeRegistrationDTO[]>([]);
    const { setSelectedTimeRegistration, setOpenEditTimeRegistrationModal, setProjects, projects } = useProjectContext();

      useEffect(() => {
        if (project == undefined) return;
        const fetchTimeRegistrations = async () => {
          try {
            console.log("new project in projectrow")
            const projectsFromApi = await timeRegistrationService.fetchTimeRegistrationsByProjectId(project.id);
            setTimeRegistrations(projectsFromApi);
          } catch (error) {
            setTimeRegistrations([]);
            console.error("Error fetching projects:", error);
          }
        };
        fetchTimeRegistrations();
      }, [project]);

    const handleEdit = (timeRegistration: TimeRegistrationDTO) => {
      setSelectedTimeRegistration(timeRegistration);
      setOpenEditTimeRegistrationModal(true);
    };
  
    const handleDelete = async (timeRegistrationId : string) => {
      await timeRegistrationService.deleteTimeRegistration(timeRegistrationId);
      setSelectedTimeRegistration(null);
      setProjects(projects.map(p => ({ ...p })));
    };
  
    return(
      <div style={{display: 'flex', flexDirection: 'column'}}>
        <div style={{
            display: 'flex',
            flexDirection: 'row',
            justifyContent: 'space-between',
            alignItems: 'center',
            padding: '10px',
            borderBottom: '1px solid #ddd',
            backgroundColor: '#e0e0e0',
            fontSize: '14px',
          }}>
            <div style={{width: 300, padding: '5px' }}><strong>Project Name: </strong> {project.name}</div>
            <div style={{width: 250, padding: '5px' }}><strong>Customer name: </strong>{project.customerName}</div>
            <div style={{width: 250, padding: '5px' }}><strong>Deadline: </strong>{project.deadline.split("T")[0]}</div>
            <div style={{width: 250, padding: '5px' }}><strong>Completed: </strong>{project.isCompleted.toString()}</div>
            <div>
              <span style={{paddingRight: 5}}><strong>Time registrations</strong></span><FontAwesomeIcon icon={isOpen ? faChevronUp : faChevronDown} onClick={() => setIsOpen(!isOpen)} style={{ cursor: 'pointer' }} />
            </div>
          </div>
          {isOpen && (
              timeRegistrations && timeRegistrations.length > 0 ? (
                <div>
                  {/* <TimeRegistrationRows onDelete={handleDelete} onEdit={handleEdit} timeRegistrations={timeRegistrations} /> */}
                </div>
              ) : (
                <h2>No time registrations found on this project!</h2>
              )
            )}
        </div>
    );
}

export default ProjectRow;

