import { useCallback, useEffect, useState } from 'react';
import { Link, useLocation } from 'react-router-dom';
import { Category } from '../../models/category';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { Button, IconButton, Pagination, PaginationItem } from '@mui/material';
import DeleteIcon from '@mui/icons-material/Delete';
import { CategoryEditor } from './category-editor/category-editor';

export function CategoryList() {
    const pageSize = 10;
    const [totalCount, setTotalCount] = useState(0);
    const [categories, setCategories] = useState<Category[]>([]);
    const [categoryForEditing, setCategoryForEditing] = useState<Category>();
    const [isEditorOpened, setIsEditorOpened] = useState<boolean>(false);

    const location = useLocation();
    const query = new URLSearchParams(location.search);
    const pageNumber = parseInt(query.get('pageNumber') || '1', 10);
    const numberOfPages = Math.ceil(totalCount / pageSize);

    useEffect(() => {
        const url = `api/category?pageNumber=${pageNumber}&pageSize=${pageSize}`;
        fetch(url)
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
                console.error(`error occurred while fetching categories, url: ${url}`);
            })
            .then((data) => {
                setCategories(data as Category[]);
                setTotalCount(data.totalCount);
            });
    }, [pageNumber]);

    const handleDeleteButtonClick = useCallback((id: number) => {
        const url = `api/category/${id}`;
        fetch(url, {
            method: 'DELETE',
        })
            // TODO: should respond with status code
            //   .then((response) => response.json())
            .then((data) => {
                if (data.ok) {
                    setCategories((current) => {
                        return current.filter((category) => category.id !== id);
                    });
                } else {
                    console.error(`error occurred while deleting author, url: ${url}`);
                }
            });
    }, [setCategories]);

    const handleItemClick = (category: Category) => {
        setCategoryForEditing(category);
        setIsEditorOpened(true);
    };

    const handleCreateButtonClick = () => {
        setIsEditorOpened(true);
    };

    const handleEditorClose = () => {
        setCategoryForEditing(undefined);
        setIsEditorOpened(false);
    };

    const handleEditorSubmit = (category: Category) => {
        setCategories((current) => {
            if (current.find((c) => c.id === category.id)) {
                return [...current.filter((c) => c.id !== category.id), category];
            }
            return [...current, category];
        });
        setIsEditorOpened(false);
    };

    return (
        <>
            <List>
                {
                    categories.map((category) => {
                        return (
                            <ListItem
                                key={category.id}>

                                <ListItemText
                                    primary={category.name}
                                    onClick={() => handleItemClick(category)}
                                />

                                <IconButton
                                    color="primary"
                                    onClick={() => handleDeleteButtonClick(category.id!)}>
                                    <DeleteIcon />
                                </IconButton>
                            </ListItem>
                        );
                    })
                }
                <ListItem>
                    <Button
                        variant="text"
                        onClick={handleCreateButtonClick}>
                        Add Category
                    </Button>
                </ListItem>
            </List>

            {numberOfPages > 1 && (
                <Pagination
                    page={pageNumber}
                    count={numberOfPages}
                    renderItem={(item) => (
                        <PaginationItem
                            component={Link}
                            to={`/categories${item.page === 1 ? '' : `?page=${item.page}`}`}
                            {...item} />
                    )}
                />
            )}

            {isEditorOpened && (
                <CategoryEditor
                    category={categoryForEditing}
                    onClose={handleEditorClose}
                    onSubmit={handleEditorSubmit} />
            )}
        </>
    );
}
