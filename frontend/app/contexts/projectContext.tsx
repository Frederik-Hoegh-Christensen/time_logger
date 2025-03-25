import { createContext, useContext, useEffect, useState } from "react";
import type { ReactNode } from "react";
import type { ProjectDTO } from "~/models/project";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";

// Define the ProjectDTO interface

// Define the context type
interface ProjectContextType {
  selectedProject: ProjectDTO | null;
  setSelectedProject: (project: ProjectDTO | null) => void;
  selectedTimeRegistration: TimeRegistrationDTO | null;
  setSelectedTimeRegistration: (project: TimeRegistrationDTO | null) => void;
  projects: ProjectDTO[];
  setProjects: (projects: ProjectDTO[]) => void;
  openEditTimeRegistrationModal: boolean;
  setOpenEditTimeRegistrationModal: (open: boolean) => void;
}

// Create the context
const ProjectContext = createContext<ProjectContextType | undefined>(undefined);

// Provider component
export const ProjectProvider = ({ children }: { children: ReactNode }) => {
  const [selectedProject, setSelectedProject] = useState<ProjectDTO | null>(null);
  const [selectedTimeRegistration, setSelectedTimeRegistration] = useState<TimeRegistrationDTO | null>(null);
  const [projects, setProjects] = useState<ProjectDTO[]>([]);
  const [openEditTimeRegistrationModal, setOpenEditTimeRegistrationModal] = useState<boolean>(false);

  
  return (
    <ProjectContext.Provider value={{ selectedProject, setSelectedProject, projects, setProjects, openEditTimeRegistrationModal, setOpenEditTimeRegistrationModal, selectedTimeRegistration, setSelectedTimeRegistration }}>
      {children}
    </ProjectContext.Provider>
  );
};

// Custom hook for using the context
export const useProjectContext = () => {
  const context = useContext(ProjectContext);
  if (!context) {
    throw new Error("useProjectContext must be used within a ProjectProvider");
  }
  return context;
};
