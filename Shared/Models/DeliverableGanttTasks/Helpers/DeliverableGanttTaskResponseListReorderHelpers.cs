using Shared.Models.DeliverableGanttTasks.Responses;

namespace Shared.Models.DeliverableGanttTasks.Helpers
{
    public static class DeliverableGanttTaskResponseListReorderHelpers
    {
        public static void Reorder(this DeliverableGanttTaskResponseList response)
        {
            int mainorder = 1;

            var deliverables = response.OrderedItems.Where(x => x.IsDeliverable).OrderBy(x => x.InternalOrder).ToList();
            foreach (var item in deliverables)
            {

                item.MainOrder = mainorder;
                mainorder++;
                if (item.HasSubTask)
                {
                    item.Reorder(ref mainorder);
                }


            }
        }
        static void Reorder(this DeliverableGanttTaskResponse task, ref int mainorder)
        {
            int internalorder = 1;

            foreach (var row in task.OrderedSubTasks)
            {
                row.ParentWBS = task.WBS;
                row.InternalOrder = internalorder;
                internalorder++;
                row.MainOrder = mainorder;
                mainorder++;
                if (row.HasSubTask)
                {
                    row.Reorder(ref mainorder);
                }


            }
        }

        public static bool DisableButtonCanMoveDown(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {

            if (selectedRow == null) return true;

            bool result = true;
            if (selectedRow.TaskParentId.HasValue)
            {
                var parent = Response.FindRow(selectedRow.TaskParentId.Value);
                if (parent != null)
                {
                    result = parent.LastMainOrder == selectedRow.MainOrder;
                }
            }
            return result;
        }
        public static bool DisableButtonCanMoveUp(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {

            if (selectedRow == null) return true;

            bool result = true;
            if (selectedRow.TaskParentId.HasValue)
            {
                var parent = Response.FindRow(selectedRow.TaskParentId.Value);
                if (parent != null)
                {
                    result = parent.FirstMainOrder == selectedRow.MainOrder;
                }
            }

            return result;
        }
        public static bool DisableButtonCanMoveRight(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            // Validación inicial: Si no hay fila seleccionada, no se puede mover
            if (selectedRow == null) return true;


            var deliverable = Response.FindRow(selectedRow.DeliverableId);

            if (deliverable == null) return true;

            // Obtener la lista plana de todos los elementos
            var flatlist = deliverable.SubTasks;
            if (!flatlist.Any(x => x.Id == selectedRow.Id)) return true;//verifica que la fila seleccionada este en la listaplana
            // Obtener el índice de la fila seleccionada en la lista plana


            int currentIndex = flatlist.FindIndex(x => x.Id == selectedRow.Id);
            // Si no hay fila anterior, no se puede mover a la derecha
            if (currentIndex == 0) return true;

            // Obtener la fila actual con el indice de la fila seleccionada en la lista plana
            var currentRow = flatlist[currentIndex];


            // Obtener la fila anterior
            var previousRow = flatlist[currentIndex - 1];

            // No permitir moverse hacia la derecha si la fila anterior es igual a la fila seleccionada
            if (currentRow == previousRow) return true;

            // Encontrar el padre actual del SelectedRow
            var currentParent = Response.FindRow(currentRow.TaskParentId);

            // Validar si el padre actual tiene subdeliverables
            if (currentParent?.SubTasks != null)
            {
                // Si el SelectedRow es el primer hijo del padre, no permitir el movimiento
                if (currentParent.OrderedSubTasks.FirstOrDefault() == currentRow)
                {
                    return true; // No permitir mover el primer hijo hacia la derecha
                }
            }

            // Verificar si la fila anterior puede actuar como un nuevo padre
            // La fila anterior puede actuar como padre si:
            // 1. Tiene una lista de SubGanttTasks (incluso si está vacía).
            // 2. No es el mismo que el padre actual del SelectedRow.
            if (previousRow.SubTasks == null || previousRow == currentParent)
            {
                return true; // La fila anterior no puede actuar como padre
            }

            // No permitir moverse hacia la derecha si la fila seleccionada ya es hija de la fila anterior
            if (previousRow.SubTasks.Contains(currentRow))
            {
                return true;
            }

            // Permitir el movimiento si todas las condiciones anteriores se cumplen
            return false;
        }
        public static bool DisableButtonCanMoveLeft(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            // Validación inicial: Si no hay fila seleccionada, no se puede mover
            if (selectedRow == null) return true;


            var deliverable = Response.FindRow(selectedRow.TaskParentId);

            if (deliverable == null) return true;

            // Obtener la lista plana de todos los elementos
            var flatlist = deliverable.SubTasks;
            if (!flatlist.Any(x => x.Id == selectedRow.Id)) return true;//verifica que la fila seleccionada este en la listaplana
            // Obtener el índice de la fila seleccionada en la lista plana

            int currentIndex = flatlist.FindIndex(x => x.Id == selectedRow.Id);
            // Si no hay fila anterior, no se puede mover a la derecha


            // Obtener la fila actual con el indice de la fila seleccionada en la lista plana
            var currentRow = flatlist[currentIndex];

            var currentParent = Response.FindRow(currentRow.TaskParentId);

            if (currentParent == null||currentParent.IsDeliverable) return true;

            // Permitir el movimiento si todas las condiciones anteriores se cumplen
            return false;
        }
        public static bool MoveUp(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            if (selectedRow == null) return false;

            //Confirmar la fila seleccionada esta en la lista
            var currentrow = Response.FindRow(selectedRow.Id);

            if (currentrow == null) return false;


            var parent = Response.FindRow(currentrow.TaskParentId);
            if (parent == null) return false;

            var list = parent.SubTasks;

            var previous = list.FirstOrDefault(x => x.InternalOrder == currentrow.InternalOrder - 1);
            if (previous == null) return false;

            var currentorder = previous.InternalOrder;
            currentrow.InternalOrder = currentorder;
            previous.InternalOrder = currentorder + 1;

            return true;
        }
        /// <summary>
        /// Mueve un elemento hacia abajo en su lista.
        /// </summary>
        public static bool MoveDown(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            if (selectedRow == null) return false;

            //Confirmar la fila seleccionada esta en la lista
            var currentrow = Response.FindRow(selectedRow.Id);

            if (currentrow == null) return false;

            // Obtener la fila actual con el indice de la fila seleccionada en la lista plana

            var parent = Response.FindRow(currentrow.TaskParentId);
            if (parent == null) return false;
            var list = parent.SubTasks;

            var next = list.FirstOrDefault(x => x.InternalOrder == currentrow.InternalOrder + 1);
            if (next == null) return false;

            var currentorder = next.InternalOrder;
            currentrow.InternalOrder = currentorder;
            next.InternalOrder = currentorder - 1;

            return true;
        }
        public static bool MoveLeft(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            if (selectedRow == null) return false;

            //Confirmar la fila seleccionada esta en la lista
            var currentrow = Response.FindRow(selectedRow.Id);

            if (currentrow == null) return false;

            // Obtener la fila actual con el indice de la fila seleccionada en la lista plana

            var parent = Response.FindRow(currentrow.TaskParentId);


            if (parent == null) return false; // Ya está en el nivel raíz, no se puede mover más a la izquierda

            // Eliminar el SelectedRow de su ubicación actual
            parent.RemoveSubTask(currentrow);

            // Encontrar el padre del padre actual
            var parentOfCurrentParent = Response.FindRow(parent.TaskParentId);

            if (parentOfCurrentParent != null)
            {
                parentOfCurrentParent.AddSubTask(currentrow);



            }

            return true;

        }
        public static bool MoveRight(this DeliverableGanttTaskResponseList Response, DeliverableGanttTaskResponse selectedRow)
        {
            if (selectedRow == null) return false;

            //Confirmar la fila seleccionada esta en la lista
            var currentrow = Response.FindRow(selectedRow.Id);

            if (currentrow == null) return false;

            var selectedindex = Response.Items.IndexOf(currentrow);

            // Encontrar el primer ancestro común en la jerarquía
            DeliverableGanttTaskResponse? ancestor = currentrow.FindAncestor(Response.Items, selectedindex);

            if (ancestor == null)
            {
                // Si no hay ances  tro, no se puede mover a la derecha
                return false;
            }

            // Remover el selectedRow de su ubicación actual
            var currentParent = Response.FindRow(currentrow.TaskParentId);

            if (currentParent != null)
            {
                currentParent.RemoveSubTask(currentrow);

            }
            if(currentrow.NewDependencies.Any(x => x.DependencyTaskId== ancestor.Id))
            {
                var dependency = currentrow.NewDependencies.FirstOrDefault(x => x.DependencyTaskId == ancestor.Id);
                currentrow.RemoveDependency(dependency!);
                
            }

            ancestor.AddSubTask(currentrow);
            return true;
        }

        private static DeliverableGanttTaskResponse? FindAncestor(this DeliverableGanttTaskResponse selectedRow, List<DeliverableGanttTaskResponse> flatList,
            int currentIndex)
        {
            // Recorrer la lista plana hacia atrás para encontrar el primer ancestro
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                var candidate = flatList[i];

                // Verificar si el candidato es un ancestro válido
                if (IsAncestor(candidate, selectedRow))
                {
                    return candidate;
                }
            }

            return null; // No se encontró un ancestro válido
        }
        private static bool IsAncestor(DeliverableGanttTaskResponse candidate, DeliverableGanttTaskResponse selectedRow)
        {
            // Un candidato es un ancestro si está en un nivel superior en la jerarquía
            // y no es un descendiente del selectedRow.

            var anterior = candidate.WBS.Split('.').Length;
            var selected = selectedRow.WBS.Split('.').Length;
            return anterior == selected;
        }
    }
}
