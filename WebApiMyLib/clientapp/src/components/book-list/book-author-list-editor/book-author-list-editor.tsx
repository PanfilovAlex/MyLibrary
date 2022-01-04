import Autocomplete, { createFilterOptions } from '@mui/material/Autocomplete';
import TextField from '@mui/material/TextField';
import { useEffect, useState } from 'react';
import { Author } from '../../../models/author';
import { getFullName } from '../../../selectors/author-selectors';

export type BookAuthorsEditorProps = {
    authors: Author[];
    onChange?: (authors: Author[]) => void;
    className?: string;
};

const filter = createFilterOptions<Author>();

export function BookAuthorListEditor(props: BookAuthorsEditorProps): JSX.Element {
    const {
        authors,
        onChange,
        className,
    } = props;

    const [avaialableAuthors, setAvailableAuthors] = useState<Author[]>();
    const [isOpened, setIsOpened] = useState<boolean>(false);
    const isLoading = !!avaialableAuthors;

    useEffect(() => {
        fetch('api/author')
            .then((response) => {
                if (response.ok) {
                    return response.json();
                }
            })
            .then((data) => {
                setAvailableAuthors(data as Author[]);
            })
    }, []);

    // TODO: something goes wrong when adding custom author
    return (
        <>
            <Autocomplete
                multiple
                options={avaialableAuthors ?? []}
                value={authors}
                // defaultValue={[...authors]}
                open={isOpened}
                onOpen={() => {
                    setIsOpened(true);
                }}
                onClose={() => {
                    setIsOpened(false);
                }}
                onChange={(event, newValue) => {
                    onChange && onChange(newValue);
                }}
                isOptionEqualToValue={(option, value) => option.id === value.id}
                filterOptions={(options, params) => {
                    const filtered = filter(options, params);

                    const { inputValue } = params;
                    const [firstName, lastName] = inputValue.split(' ');
                    // Suggest the creation of a new value
                    const isExisting = options.some((option) => inputValue === getFullName(option));
                    if (inputValue !== '' && !isExisting) {
                        filtered.push({
                            firstName: firstName ?? '',
                            lastName: lastName ?? '',
                            books: [],
                        });
                    }

                    return filtered;
                }}
                getOptionLabel={(option) => {
                    // Value selected with enter, right from the input
                    if (typeof option === 'string') {
                        return option;
                    }
                    // Add "xxx" option created dynamically
                    if (!option.id) {
                        return getFullName(option);
                    }
                    // Regular option
                    return getFullName(option);
                }}
                loading={isLoading}
                renderInput={(params) => (
                    <TextField {...params} label="Authors" />
                )}
                className={className}
            />
        </>
    );
}
