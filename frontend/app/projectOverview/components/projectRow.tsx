import { useEffect, useState } from "react";
import type { ProjectDTO } from "~/models/project";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronDown, faChevronUp, faPencil, faTrash } from '@fortawesome/free-solid-svg-icons';
import type { TimeRegistrationDTO } from "~/models/timeRegistration";
import TimeRegistrationRows from "../../sharedComponents/timeRegistrationRows";
import { timeRegistrationService } from "~/api/timeRegistrationService";
import { useProjectContext } from "~/contexts/projectContext";
import EditProjectModal from "./EditProjectModal";
import ActionConfirmationDialog from "~/sharedComponents/ActionConfirmationDialog";
import { projectService } from "~/api/projectService";

interface IProps {
    project: ProjectDTO;
}
const ProjectRow: React.FC<IProps> = ({ project}) => {
    const [isOpen, setIsOpen] = useState<boolean>(false);
    const [openProjectEdit, setOpenProjectEdit] = useState<boolean>(false);
    const [openDeleteDialog, setOpenDeleteDialog] = useState<boolean>(false);
    const [timeRegistrations, setTimeRegistrations] = useState<TimeRegistrationDTO[]>([]);
    const { setSelectedTimeRegistration, setProjects, projects } = useProjectContext();

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
    

    const handleConfirmEdit = () => {
      setProjects(projects.map(p => ({ ...p })));
    };

    const handleProjectConfirmEdit = (updatedProject: ProjectDTO) => {
      setProjects((prevProjects: ProjectDTO[]) =>
        prevProjects.map(p => (p.id === updatedProject.id ? updatedProject : p))
      );
      setOpenProjectEdit(false);
    };
  
    const handleTrDelete = async (timeRegistrationId : string) => {
      await timeRegistrationService.deleteTimeRegistration(timeRegistrationId);
      setSelectedTimeRegistration(null);
      setProjects(projects.map(p => ({ ...p })));
    };

    const handleProjectDelete = async (projectId : string) => {
      await projectService.deleteProject(projectId);
      setProjects((prevProjects: ProjectDTO[]) =>
        prevProjects.filter(p => p.id !== projectId)
      );
      setOpenDeleteDialog(false)
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
            <div>
            <button style={{ cursor: "pointer", padding: 10 }} onClick={() => setOpenProjectEdit(true)}>
              <FontAwesomeIcon icon={faPencil} />
            </button>
            <button style={{ cursor: "pointer", padding: 10 }} onClick={() => setOpenDeleteDialog(true)}>
              <FontAwesomeIcon icon={faTrash} />
            </button>
          </div>
          </div>
          {isOpen && (
              timeRegistrations && timeRegistrations.length > 0 ? (
                <div>
                  <TimeRegistrationRows onDelete={handleTrDelete} onConfirmEdit={handleConfirmEdit} timeRegistrations={timeRegistrations} />
                </div>
              ) : (
                <h2>No time registrations found on this project!</h2>
              )
            )}
          <ActionConfirmationDialog
            title="Are you sure you want to delete this Project and all its related time registrations?"
            isOpen={openDeleteDialog}
            onClose={() => setOpenDeleteDialog(false)}
            onConfirm={() => handleProjectDelete(project.id)}
          />
          <EditProjectModal project={project} onClose={() => setOpenProjectEdit(false)} open={openProjectEdit} onConfirm={handleProjectConfirmEdit}/>
        </div>
    );
}

export default ProjectRow;

