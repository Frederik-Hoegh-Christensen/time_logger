import { useId } from 'react';
import Select from 'react-select';
import type { ProjectDTO } from '~/models/project';

const customStyles = {
  control: (provided: any) => ({
    ...provided,
    width: '300px', // You can change this width to whatever you need
  }),
};

interface IProps {
  projects: ProjectDTO[];
  selectedProject: ProjectDTO | null;
  onProjectSelect: (project: ProjectDTO) => void;
}

const ProjectDropDown: React.FC<IProps> = ({ projects, selectedProject, onProjectSelect }) => {
  const selectId = useId();
  const inCompletedProjects = projects.filter(p => p.isCompleted === false);
  const mappedProjects = inCompletedProjects.map(p => ({ value: p, label: p.name }));

  return (
    <div className="dropdown-container">
      <Select
        inputId={selectId}
        options={mappedProjects}
        placeholder="Select Project"
        styles={customStyles}
        value={selectedProject ? { value: selectedProject, label: selectedProject.name } : null}
        onChange={(p) => p && onProjectSelect(p.value)}
        getOptionLabel={(e) => e.label} // Ensure the correct label is displayed
      />
    </div>
  );
};

export default ProjectDropDown;
