import type { ProjectCreateDTO, ProjectDTO } from "~/models/project";
import { apiService } from "./apiService";
import type { TimeRegistrationDTO } from "~/models/timeRegistration";

export const projectService = {
  // Fetch Project by ID
  fetchProject: async (projectId: string): Promise<ProjectDTO> => {
    return await apiService.get<ProjectDTO>(`/projects/${projectId}`);
  },

  // Fetch Time Registrations by Project ID
  fetchTimeRegistrationsForProject: async (projectId: string): Promise<TimeRegistrationDTO[]> => {
    return await apiService.get<TimeRegistrationDTO[]>(`/projects/${projectId}/timeregistrations`);
  },

  // Create a new Project
  createProject: async (projectData: ProjectCreateDTO): Promise<void> => {
    return await apiService.post("/projects", projectData);
  },

  // Update an existing Project
  updateProject: async (projectId: string, updatedProject: ProjectDTO): Promise<void> => {
    return await apiService.put(`/projects/${projectId}`, updatedProject);
  },

  // Delete a Project by ID
  deleteProject: async (projectId: string): Promise<void> => {
    return await apiService.delete(`/projects/${projectId}`);
  },
};
